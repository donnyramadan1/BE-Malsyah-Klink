using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
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
    public class AttendanceLogService : IAttendanceLogService
    {
        private readonly IAttendanceLogRepository _attendanceLogRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AttendanceLogService> _logger;

        public AttendanceLogService(
            IAttendanceLogRepository attendanceLogRepository,
            IMapper mapper,
            ILogger<AttendanceLogService> logger)
        {
            _attendanceLogRepository = attendanceLogRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AttendanceLogDto>> GetAllAttendanceLogsAsync()
        {
            try
            {
                var logs = await _attendanceLogRepository.GetAllWithIncludesAsync(al => al.User);
                return _mapper.Map<IEnumerable<AttendanceLogDto>>(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil data semua log presensi.");
                throw new ApplicationException("Terjadi kesalahan saat mengambil data log presensi.");
            }
        }

        public async Task<IEnumerable<AttendanceLogDto>> GetAttendanceLogsByUserIdAsync(int userId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var logs = await _attendanceLogRepository.GetByUserIdAsync(userId, startDate, endDate);
                return _mapper.Map<IEnumerable<AttendanceLogDto>>(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil log presensi untuk user ID: {UserId}", userId);
                throw new ApplicationException("Terjadi kesalahan saat mengambil log presensi.");
            }
        }

        public async Task LogAttendanceAttemptAsync(int userId, string action, string status, string reason,
            decimal? latitude = null, decimal? longitude = null, string faceImage = null)
        {
            try
            {
                await _attendanceLogRepository.LogAttendanceAttemptAsync(
                    userId, action, status, reason, latitude, longitude, faceImage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error logging attendance attempt for user ID {userId}");
                throw;
            }
        }
    }
}
