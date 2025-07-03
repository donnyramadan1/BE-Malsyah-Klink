using APIKlinik.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(int id);
        Task<RoleDto> AddRoleAsync(CreateRoleDto createRoleDto);
        Task UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto);
        Task DeleteRoleAsync(int id);
    }
}
