using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIKlinik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> Get()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(ApiResponse<IEnumerable<RoleDto>>.Success(roles));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<RoleDto>>> Get(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound(ApiResponse<RoleDto>.NotFound("Role not found"));

            return Ok(ApiResponse<RoleDto>.Success(role));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<RoleDto>>> Post([FromBody] CreateRoleDto createRoleDto)
        {
            var role = await _roleService.AddRoleAsync(createRoleDto);
            return CreatedAtAction(nameof(Get), new { id = role.Id },
                ApiResponse<RoleDto>.Success("Role created successfully", role));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Put(int id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            await _roleService.UpdateRoleAsync(id, updateRoleDto);
            return Ok(ApiResponse<bool>.Success("Role updated successfully", true));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            await _roleService.DeleteRoleAsync(id);
            return Ok(ApiResponse<bool>.Success("Role deleted successfully", true));
        }
    }
}
