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
    public class UserShiftService : IUserShiftService
    {
        private readonly IUserShiftRepository _userShiftRepository;
        private readonly IUserRepository _userRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserShiftService> _logger;

        public UserShiftService(
            IUserShiftRepository userShiftRepository,
            IUserRepository userRepository,
            IShiftRepository shiftRepository,
            IMapper mapper,
            ILogger<UserShiftService> logger)
        {
            _userShiftRepository = userShiftRepository;
            _userRepository = userRepository;
            _shiftRepository = shiftRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserShiftDto>> GetAllUserShiftsAsync()
        {
            try
            {
                var userShifts = await _userShiftRepository.GetAllWithIncludesAsync(
                    us => us.User,
                    us => us.Shift);

                return _mapper.Map<IEnumerable<UserShiftDto>>(userShifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil data semua user shift.");
                throw new ApplicationException("Terjadi kesalahan saat mengambil data user shift.");
            }
        }

        public async Task<UserShiftDto> GetUserShiftByIdAsync(int id)
        {
            try
            {
                var userShift = await _userShiftRepository.GetByIdAsync(id);
                if (userShift == null)
                    throw new ApplicationException("User shift tidak ditemukan.");

                return _mapper.Map<UserShiftDto>(userShift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil user shift dengan ID: {UserShiftId}", id);
                throw new ApplicationException("Terjadi kesalahan saat mengambil detail user shift.");
            }
        }

        public async Task<IEnumerable<UserShiftDto>> GetUserShiftsByUserIdAsync(int userId)
        {
            try
            {
                var userShifts = await _userShiftRepository.GetByUserIdAsync(userId);
                return _mapper.Map<IEnumerable<UserShiftDto>>(userShifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil user shift untuk user ID: {UserId}", userId);
                throw new ApplicationException("Terjadi kesalahan saat mengambil user shift.");
            }
        }

        public async Task<UserShiftDto> AddUserShiftAsync(CreateUserShiftDto createUserShiftDto)
        {
            try
            {
                // Validasi user exists
                var user = await _userRepository.GetByIdAsync(createUserShiftDto.UserId);
                if (user == null)
                    throw new ApplicationException("User tidak ditemukan.");

                // Validasi shift exists
                var shift = await _shiftRepository.GetByIdAsync(createUserShiftDto.ShiftId);
                if (shift == null)
                    throw new ApplicationException("Shift tidak ditemukan.");

                // Validasi tidak ada duplikasi
                if (await _userShiftRepository.IsUserShiftExists(createUserShiftDto.UserId, createUserShiftDto.Date))
                    throw new ApplicationException("User sudah memiliki shift pada tanggal tersebut.");

                var userShift = _mapper.Map<UserShift>(createUserShiftDto);
                await _userShiftRepository.AddAsync(userShift);
                return _mapper.Map<UserShiftDto>(userShift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menambahkan user shift baru.");
                throw new ApplicationException("Terjadi kesalahan saat menambahkan user shift.");
            }
        }

        public async Task UpdateUserShiftAsync(UpdateUserShiftDto updateUserShiftDto)
        {
            try
            {
                var userShift = await _userShiftRepository.GetByIdAsync(updateUserShiftDto.Id);
                if (userShift == null)
                    throw new ApplicationException("User shift tidak ditemukan untuk diperbarui.");

                // Validasi shift exists
                var shift = await _shiftRepository.GetByIdAsync(updateUserShiftDto.ShiftId);
                if (shift == null)
                    throw new ApplicationException("Shift tidak ditemukan.");

                // Validasi tidak ada duplikasi
                if (await _userShiftRepository.IsUserShiftExists(updateUserShiftDto.UserId, updateUserShiftDto.Date)
                    && (userShift.Date != updateUserShiftDto.Date || userShift.UserId != updateUserShiftDto.UserId))
                    throw new ApplicationException("User sudah memiliki shift pada tanggal tersebut.");

                _mapper.Map(updateUserShiftDto, userShift);
                await _userShiftRepository.UpdateAsync(userShift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memperbarui user shift dengan ID: {UserShiftId}", updateUserShiftDto.Id);
                throw;
            }
        }

        public async Task DeleteUserShiftAsync(int id)
        {
            try
            {
                var userShift = await _userShiftRepository.GetByIdAsync(id);
                if (userShift == null)
                    throw new ApplicationException("User shift tidak ditemukan untuk dihapus.");

                await _userShiftRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menghapus user shift dengan ID: {UserShiftId}", id);
                throw new ApplicationException("Terjadi kesalahan saat menghapus user shift.");
            }
        }
    }
}
