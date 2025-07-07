using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIKlinik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRolesController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserRoleDto>>>> Get()
        {
            try
            {
                var userRoles = await _userRoleService.GetAllUserRolesAsync();
                return Ok(ApiResponse<IEnumerable<UserRoleDto>>.Success("Berhasil mengambil data user role", userRoles));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<UserRoleDto>>.InternalError("Terjadi kesalahan saat mengambil data user role: " + ex.Message));
            }
        }

        [HttpPost("assign")]
        public async Task<ActionResult<ApiResponse<bool>>> AssignRoleToUser([FromBody] AssignUserRoleDto assignUserRoleDto)
        {
            try
            {
                await _userRoleService.AssignRoleToUser(assignUserRoleDto);
                return Ok(ApiResponse<bool>.Success("Role berhasil ditetapkan ke user", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal menetapkan role ke user: " + ex.Message));
            }
        }

        [HttpDelete("remove/{userId}/{roleId}")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveRoleFromUser(int userId, int roleId)
        {
            try
            {
                await _userRoleService.RemoveRoleFromUser(userId, roleId);
                return Ok(ApiResponse<bool>.Success("Role berhasil dihapus dari user", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal menghapus role dari user: " + ex.Message));
            }
        }
    }
}
