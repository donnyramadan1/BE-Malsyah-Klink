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
            var userRoles = await _userRoleService.GetAllUserRolesAsync();
            return Ok(ApiResponse<IEnumerable<UserRoleDto>>.Success(userRoles));
        }

        [HttpPost("assign")]
        public async Task<ActionResult<ApiResponse<bool>>> AssignRoleToUser([FromBody] AssignUserRoleDto assignUserRoleDto)
        {
            await _userRoleService.AssignRoleToUser(assignUserRoleDto);
            return Ok(ApiResponse<bool>.Success("Role assigned to user successfully", true));
        }

        [HttpDelete("remove/{userId}/{roleId}")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveRoleFromUser(int userId, int roleId)
        {
            await _userRoleService.RemoveRoleFromUser(userId, roleId);
            return Ok(ApiResponse<bool>.Success("Role removed from user successfully", true));
        }
    }
}
