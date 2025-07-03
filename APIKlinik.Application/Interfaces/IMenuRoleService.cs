
using APIKlinik.Application.DTOs;

namespace APIKlinik.Application.Interfaces
{
    public interface IMenuRoleService
    {
        Task<IEnumerable<MenuRoleDto>> GetAllMenuRolesAsync();
        Task AssignMenuToRole(AssignMenuRoleDto assignMenuRoleDto);
        Task RemoveMenuFromRole(int menuId, int roleId);
    }
}
