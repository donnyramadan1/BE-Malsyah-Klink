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
            try
            {
                var menuRoles = await _menuRoleService.GetAllMenuRolesAsync();
                return Ok(ApiResponse<IEnumerable<MenuRoleDto>>.Success("Berhasil mengambil data menu role", menuRoles));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<MenuRoleDto>>.InternalError("Terjadi kesalahan saat mengambil data menu role: " + ex.Message));
            }
        }

        [HttpPost("assign")]
        public async Task<ActionResult<ApiResponse<bool>>> AssignMenuToRole([FromBody] AssignMenuRoleDto assignMenuRoleDto)
        {
            try
            {
                await _menuRoleService.AssignMenuToRole(assignMenuRoleDto);
                return Ok(ApiResponse<bool>.Success("Menu berhasil ditetapkan ke role", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal menetapkan menu ke role: " + ex.Message));
            }
        }

        [HttpDelete("remove/{menuId}/{roleId}")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveMenuFromRole(int menuId, int roleId)
        {
            try
            {
                await _menuRoleService.RemoveMenuFromRole(menuId, roleId);
                return Ok(ApiResponse<bool>.Success("Menu berhasil dihapus dari role", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal menghapus menu dari role: " + ex.Message));
            }
        }
    }
}
