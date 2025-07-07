using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Domain.Entities;
using APIKlinik.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace APIKlinik.Application.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserRoleService> _logger;

        public UserRoleService(IRepository<UserRole> userRoleRepository, IMapper mapper, ILogger<UserRoleService> logger)
        {
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserRoleDto>> GetAllUserRolesAsync()
        {
            try
            {
                var userRoles = await _userRoleRepository.GetAllWithIncludesAsync(
                    ur => ur.User,
                    ur => ur.Role
                );

                return _mapper.Map<IEnumerable<UserRoleDto>>(userRoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil data user-role.");
                throw new ApplicationException("Terjadi kesalahan saat mengambil data user-role.");
            }
        }

        public async Task AssignRoleToUser(AssignUserRoleDto assignUserRoleDto)
        {
            try
            {
                // Cek apakah sudah ada relasi user-role tersebut
                var existing = await _userRoleRepository.FindAsync(ur =>
                    ur.UserId == assignUserRoleDto.UserId &&
                    ur.RoleId == assignUserRoleDto.RoleId);

                if (existing.Any())
                {
                    throw new ApplicationException("Role sudah ditetapkan ke user ini.");
                }

                var userRole = _mapper.Map<UserRole>(assignUserRoleDto);
                await _userRoleRepository.AddAsync(userRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menetapkan role ke user.");
                throw new ApplicationException("Gagal menetapkan role ke user.");
            }
        }

        public async Task RemoveRoleFromUser(int userId, int roleId)
        {
            try
            {
                var userRole = (await _userRoleRepository.FindAsync(ur =>
                    ur.UserId == userId && ur.RoleId == roleId)).FirstOrDefault();

                if (userRole == null)
                {
                    throw new ApplicationException("Relasi user-role tidak ditemukan.");
                }

                await _userRoleRepository.DeleteAsync(userRole.RoleId); // atau DeleteAsync(userRole)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menghapus role dari user.");
                throw new ApplicationException("Gagal menghapus role dari user.");
            }
        }
    }
}
