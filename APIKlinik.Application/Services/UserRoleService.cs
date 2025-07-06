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
    public class UserRoleService : IUserRoleService
    {
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IMapper _mapper;

        public UserRoleService(IRepository<UserRole> userRoleRepository, IMapper mapper)
        {
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserRoleDto>> GetAllUserRolesAsync()
        {
            var userRoles = await _userRoleRepository.GetAllWithIncludesAsync(
                            mr => mr.User,
                            mr => mr.Role
                        );
            return _mapper.Map<IEnumerable<UserRoleDto>>(userRoles);
        }

        public async Task AssignRoleToUser(AssignUserRoleDto assignUserRoleDto)
        {
            var userRole = _mapper.Map<UserRole>(assignUserRoleDto);
            await _userRoleRepository.AddAsync(userRole);
        }

        public async Task RemoveRoleFromUser(int userId, int roleId)
        {
            var userRole = (await _userRoleRepository.FindAsync(ur =>
                ur.UserId == userId && ur.RoleId == roleId)).FirstOrDefault();

            if (userRole != null)
            {
                await _userRoleRepository.DeleteAsync(userRole.RoleId);
            }
        }
    }
}
