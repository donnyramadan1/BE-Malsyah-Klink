using APIKlinik.Application.DTOs;
using APIKlinik.Domain.Entities;

namespace APIKlinik.Application.Interfaces
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRoleDto>> GetAllUserRolesAsync();
        Task<PagedResult<UserRoleDto>> GetPagedUserRolesAsync(int page, int pageSize);

        Task AssignRoleToUser(AssignUserRoleDto assignUserRoleDto);
        Task RemoveRoleFromUser(int userId, int roleId);
    }
}
