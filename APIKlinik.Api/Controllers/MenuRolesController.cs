using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIKlinik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MenuRolesController : ControllerBase
    {
        private readonly IMenuRoleService _menuRoleService;

        public MenuRolesController(IMenuRoleService menuRoleService)
        {
            _menuRoleService = menuRoleService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<MenuRoleDto>>>> Get()
        {
            var menuRoles = await _menuRoleService.GetAllMenuRolesAsync();
            return Ok(ApiResponse<IEnumerable<MenuRoleDto>>.Success(menuRoles));
        }

        [HttpPost("assign")]
        public async Task<ActionResult<ApiResponse<bool>>> AssignMenuToRole([FromBody] AssignMenuRoleDto assignMenuRoleDto)
        {
            await _menuRoleService.AssignMenuToRole(assignMenuRoleDto);
            return Ok(ApiResponse<bool>.Success("Menu assigned to role successfully", true));
        }

        [HttpDelete("remove/{menuId}/{roleId}")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveMenuFromRole(int menuId, int roleId)
        {
            await _menuRoleService.RemoveMenuFromRole(menuId, roleId);
            return Ok(ApiResponse<bool>.Success("Menu removed from role successfully", true));
        }
    }
}
