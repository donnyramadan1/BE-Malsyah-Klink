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
        public async Task<ActionResult<IEnumerable<MenuRoleDto>>> Get()
        {
            var menuRoles = await _menuRoleService.GetAllMenuRolesAsync();
            return Ok(menuRoles);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignMenuToRole([FromBody] AssignMenuRoleDto assignMenuRoleDto)
        {
            await _menuRoleService.AssignMenuToRole(assignMenuRoleDto);
            return NoContent();
        }

        [HttpDelete("remove/{menuId}/{roleId}")]
        public async Task<IActionResult> RemoveMenuFromRole(int menuId, int roleId)
        {
            await _menuRoleService.RemoveMenuFromRole(menuId, roleId);
            return NoContent();
        }
    }
}
