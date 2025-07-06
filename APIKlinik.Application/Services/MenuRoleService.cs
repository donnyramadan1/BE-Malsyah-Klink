using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Domain.Entities;
using APIKlinik.Domain.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Services
{
    public class MenuRoleService : IMenuRoleService
    {
        private readonly IRepository<MenuRole> _menuRoleRepository;
        private readonly IMapper _mapper;

        public MenuRoleService(IRepository<MenuRole> menuRoleRepository, IMapper mapper)
        {
            _menuRoleRepository = menuRoleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MenuRoleDto>> GetAllMenuRolesAsync()
        {
            var menuRoles = await _menuRoleRepository.GetAllWithIncludesAsync(
                            mr => mr.Menu,
                            mr => mr.Role
                        );
            return _mapper.Map<IEnumerable<MenuRoleDto>>(menuRoles);
        }

        public async Task AssignMenuToRole(AssignMenuRoleDto assignMenuRoleDto)
        {
            var menuRole = _mapper.Map<MenuRole>(assignMenuRoleDto);
            await _menuRoleRepository.AddAsync(menuRole);
        }

        public async Task RemoveMenuFromRole(int menuId, int roleId)
        {
            var menuRole = (await _menuRoleRepository.FindAsync(mr =>
                mr.MenuId == menuId && mr.RoleId == roleId)).FirstOrDefault();

            if (menuRole != null)
            {
                await _menuRoleRepository.DeleteAsync(menuRole.RoleId);
            }
        }
    }
}
