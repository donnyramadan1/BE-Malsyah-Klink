using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Domain.Entities;
using APIKlinik.Infrastructure.Interface;
using APIKlinik.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IUserShiftRepository _userShiftRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IAttendanceLogRepository _attendanceLogRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AttendanceService> _logger;

        public AttendanceService(
            IAttendanceRepository attendanceRepository,
            IUserShiftRepository userShiftRepository,
            IUserRepository userRepository,
            ILocationRepository locationRepository,
            IAttendanceLogRepository attendanceLogRepository,
            IMapper mapper,
            ILogger<AttendanceService> logger)
        {
            _attendanceRepository = attendanceRepository;
            _userShiftRepository = userShiftRepository;
            _userRepository = userRepository;
            _locationRepository = locationRepository;
            _attendanceLogRepository = attendanceLogRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AttendanceDto>> GetAllAttendancesAsync()
        {
            try
            {
                var attendances = await _attendanceRepository.GetAllWithIncludesAsync(a => a.User);
                return _mapper.Map<IEnumerable<AttendanceDto>>(attendances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil data semua presensi.");
                throw new ApplicationException("Terjadi kesalahan saat mengambil data presensi.");
            }
        }

        public async Task<AttendanceDto> GetAttendanceByIdAsync(int id)
        {
            try
            {
                var attendance = await _attendanceRepository.GetByIdAsync(id);
                if (attendance == null)
                    throw new ApplicationException("Presensi tidak ditemukan.");

                return _mapper.Map<AttendanceDto>(attendance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil presensi dengan ID: {AttendanceId}", id);
                throw new ApplicationException("Terjadi kesalahan saat mengambil detail presensi.");
            }
        }

        public async Task<IEnumerable<AttendanceDto>> GetAttendancesByUserIdAsync(int userId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var attendances = await _attendanceRepository.GetByUserIdAsync(userId, startDate, endDate);
                return _mapper.Map<IEnumerable<AttendanceDto>>(attendances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil presensi untuk user ID: {UserId}", userId);
                throw new ApplicationException("Terjadi kesalahan saat mengambil presensi.");
            }
        }

        public async Task<AttendanceDto> CheckInAsync(CheckInOutDto checkInDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(checkInDto.UserId);
                if (user == null)
                    throw new ApplicationException("User tidak ditemukan.");

                var today = DateTime.Now.Date;
                if (await _attendanceRepository.HasCheckedInTodayAsync(checkInDto.UserId, today))
                    throw new ApplicationException("Anda sudah melakukan check-in hari ini.");

                // Validasi lokasi
                var clinicLocation = await _locationRepository.GetClinicLocationAsync();
                if (clinicLocation == null)
                    throw new ApplicationException("Lokasi klinik belum diatur.");

                var distance = CalculateDistance(
                    (double)checkInDto.Latitude,
                    (double)checkInDto.Longitude,
                    (double)clinicLocation.Latitude,
                    (double)clinicLocation.Longitude);

                if (distance > clinicLocation.RadiusInMeters)
                {
                    await LogFailedAttendance(checkInDto.UserId, "checkin", "Lokasi di luar radius yang diizinkan");
                    throw new ApplicationException($"Anda berada di luar radius presensi. Jarak: {distance}m (Maks: {clinicLocation.RadiusInMeters}m)");
                }

                // Validasi jam shift
                var userShift = await _userShiftRepository.GetByUserAndDateAsync(checkInDto.UserId, DateTime.Now);
                if (userShift == null)
                {
                    await LogFailedAttendance(checkInDto.UserId, "checkin", "Shift tidak ditemukan");
                    throw new ApplicationException("Shift tidak ditemukan untuk hari ini.");
                }

                var now = DateTime.Now;
                var shiftStartTime = now.Date.Add(userShift.Shift.StartTime);
                var lateThreshold = shiftStartTime.AddMinutes(userShift.Shift.LateToleranceMinutes);
                var isLate = now > lateThreshold;

                var attendance = new Attendance
                {
                    UserId = checkInDto.UserId,
                    Date = today,
                    CheckinTime = now,
                    Status = "hadir",
                    Latitude = checkInDto.Latitude,
                    Longitude = checkInDto.Longitude,
                    FaceImage = checkInDto.FaceImage,
                    IsLate = isLate
                };

                await _attendanceRepository.AddAsync(attendance);
                await LogAttendance(checkInDto.UserId, "checkin", "success");

                return _mapper.Map<AttendanceDto>(attendance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal melakukan check-in untuk user ID: {UserId}", checkInDto.UserId);
                throw;
            }
        }

        public async Task<AttendanceDto> CheckOutAsync(CheckInOutDto checkOutDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(checkOutDto.UserId);
                if (user == null)
                    throw new ApplicationException("User tidak ditemukan.");

                var today = DateTime.Now.Date;
                var attendance = await _attendanceRepository.GetByUserAndDateAsync(checkOutDto.UserId, today);
                if (attendance == null)
                    throw new ApplicationException("Anda belum melakukan check-in hari ini.");

                if (attendance.CheckoutTime != null)
                    throw new ApplicationException("Anda sudah melakukan check-out hari ini.");

                // Validasi lokasi
                var clinicLocation = await _locationRepository.GetClinicLocationAsync();
                if (clinicLocation == null)
                    throw new ApplicationException("Lokasi klinik belum diatur.");

                var distance = CalculateDistance(
                    (double)checkOutDto.Latitude,
                    (double)checkOutDto.Longitude,
                    (double)clinicLocation.Latitude,
                    (double)clinicLocation.Longitude);

                if (distance > clinicLocation.RadiusInMeters)
                {
                    await LogFailedAttendance(checkOutDto.UserId, "checkout", "Lokasi di luar radius yang diizinkan");
                    throw new ApplicationException($"Anda berada di luar radius presensi. Jarak: {distance}m (Maks: {clinicLocation.RadiusInMeters}m)");
                }

                attendance.CheckoutTime = DateTime.Now;
                attendance.UpdatedAt = DateTime.Now;
                await _attendanceRepository.UpdateAsync(attendance);
                await LogAttendance(checkOutDto.UserId, "checkout", "success");

                return _mapper.Map<AttendanceDto>(attendance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal melakukan check-out untuk user ID: {UserId}", checkOutDto.UserId);
                throw;
            }
        }

        public async Task<AttendanceDto> SubmitAttendanceRequestAsync(AttendanceRequestDto requestDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(requestDto.UserId);
                if (user == null)
                    throw new ApplicationException("User tidak ditemukan.");

                var today = requestDto.Date.Date;
                var existingAttendance = await _attendanceRepository.GetByUserAndDateAsync(requestDto.UserId, today);
                if (existingAttendance != null)
                    throw new ApplicationException("Anda sudah memiliki catatan presensi untuk tanggal tersebut.");

                var attendance = new Attendance
                {
                    UserId = requestDto.UserId,
                    Date = today,
                    Status = requestDto.Status.ToLower(),
                    UpdatedAt = DateTime.Now
                };

                await _attendanceRepository.AddAsync(attendance);
                await LogAttendance(requestDto.UserId, requestDto.Status, "success", requestDto.Notes);

                return _mapper.Map<AttendanceDto>(attendance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengajukan permohonan presensi untuk user ID: {UserId}", requestDto.UserId);
                throw;
            }
        }

        private async Task LogAttendance(int userId, string action, string status, string reason = null)
        {
            var log = new AttendanceLog
            {
                UserId = userId,
                Action = action,
                Status = status,
                Reason = reason,
                CreatedAt = DateTime.Now
            };

            await _attendanceLogRepository.AddAsync(log);
        }

        private async Task LogFailedAttendance(int userId, string action, string reason)
        {
            await LogAttendance(userId, action, "failed", reason);
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Implementasi perhitungan jarak Haversine
            const double R = 6371e3; // Radius bumi dalam meter
            var φ1 = lat1 * Math.PI / 180;
            var φ2 = lat2 * Math.PI / 180;
            var Δφ = (lat2 - lat1) * Math.PI / 180;
            var Δλ = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                    Math.Cos(φ1) * Math.Cos(φ2) *
                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // Jarak dalam meter
        }

    }
}
