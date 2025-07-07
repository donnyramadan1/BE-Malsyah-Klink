using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Domain.Entities;
using APIKlinik.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace APIKlinik.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<UserDto>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil data semua user.");
                throw new ApplicationException("Terjadi kesalahan saat mengambil data user.");
            }
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                    throw new ApplicationException("User tidak ditemukan.");

                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil user dengan ID: {UserId}", id);
                throw new ApplicationException("Terjadi kesalahan saat mengambil detail user.");
            }
        }

        public async Task<UserDto> AddUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                var user = _mapper.Map<User>(createUserDto);
                await _userRepository.AddAsync(user);
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menambahkan user baru.");
                throw new ApplicationException("Terjadi kesalahan saat menambahkan user.");
            }
        }

        public async Task UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                    throw new ApplicationException("User tidak ditemukan untuk diperbarui.");

                _mapper.Map(updateUserDto, user);
                await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memperbarui user dengan ID: {UserId}", id);
                throw new ApplicationException("Terjadi kesalahan saat memperbarui user.");
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                    throw new ApplicationException("User tidak ditemukan untuk dihapus.");

                await _userRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menghapus user dengan ID: {UserId}", id);
                throw new ApplicationException("Terjadi kesalahan saat menghapus user.");
            }
        }
    }
}
