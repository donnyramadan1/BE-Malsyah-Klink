using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Domain.Entities;
using APIKlinik.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace APIKlinik.Application.Services
{
    public class MenuRoleService : IMenuRoleService
    {
        private readonly IRepository<MenuRole> _menuRoleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MenuRoleService> _logger;

        public MenuRoleService(
            IRepository<MenuRole> menuRoleRepository,
            IMapper mapper,
            ILogger<MenuRoleService> logger)
        {
            _menuRoleRepository = menuRoleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<MenuRoleDto>> GetAllMenuRolesAsync()
        {
            try
            {
                var menuRoles = await _menuRoleRepository.GetAllWithIncludesAsync(
                    mr => mr.Menu,
                    mr => mr.Role
                );

                return _mapper.Map<IEnumerable<MenuRoleDto>>(menuRoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil data MenuRole");
                throw new ApplicationException("Terjadi kesalahan saat mengambil data MenuRole");
            }
        }

        public async Task AssignMenuToRole(AssignMenuRoleDto assignMenuRoleDto)
        {
            try
            {
                var existing = await _menuRoleRepository.FindAsync(mr =>
                    mr.MenuId == assignMenuRoleDto.MenuId && mr.RoleId == assignMenuRoleDto.RoleId);

                if (existing.Any())
                {
                    throw new ApplicationException("Menu sudah ditetapkan ke role ini.");
                }

                var menuRole = _mapper.Map<MenuRole>(assignMenuRoleDto);
                await _menuRoleRepository.AddAsync(menuRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menetapkan menu ke role");
                throw new ApplicationException("Gagal menetapkan menu ke role.");
            }
        }

        public async Task RemoveMenuFromRole(int menuId, int roleId)
        {
            try
            {
                var menuRole = (await _menuRoleRepository.FindAsync(mr =>
                    mr.MenuId == menuId && mr.RoleId == roleId)).FirstOrDefault();

                if (menuRole == null)
                {
                    throw new ApplicationException("Relasi menu-role tidak ditemukan.");
                }

                await _menuRoleRepository.DeleteAsync(menuRole.RoleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menghapus menu dari role");
                throw new ApplicationException("Gagal menghapus menu dari role.");
            }
        }
    }
}
