using APIKlinik.Application.DTOs;
using APIKlinik.Domain.Entities;
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

        Task<PagedResult<RoleDto>> GetPagedRolesAsync(int page, int pageSize);

        Task<RoleDto> AddRoleAsync(CreateRoleDto createRoleDto);
        Task UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto);
        Task DeleteRoleAsync(int id);
    }
}
