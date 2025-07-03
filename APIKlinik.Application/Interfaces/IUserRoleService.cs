using APIKlinik.Application.DTOs;

namespace APIKlinik.Application.Interfaces
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRoleDto>> GetAllUserRolesAsync();
        Task AssignRoleToUser(AssignUserRoleDto assignUserRoleDto);
        Task RemoveRoleFromUser(int userId, int roleId);
    }
}
