using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Application.Services;
using APIKlinik.Domain.Entities;
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
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(ApiResponse<IEnumerable<RoleDto>>.Success("Berhasil mengambil data role", roles));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<RoleDto>>.InternalError("Terjadi kesalahan saat mengambil data role: " + ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<RoleDto>>> Get(int id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                if (role == null)
                    return NotFound(ApiResponse<RoleDto>.NotFound("Role tidak ditemukan"));

                return Ok(ApiResponse<RoleDto>.Success("Berhasil mengambil detail role", role));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<RoleDto>.InternalError("Terjadi kesalahan saat mengambil detail role: " + ex.Message));
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponse<PagedResult<RoleDto>>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            try
            {
                var result = await _roleService.GetPagedRolesAsync(page, pageSize, search);
                return Ok(ApiResponse<PagedResult<RoleDto>>.Success("Berhasil mengambil data role dengan pagination", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PagedResult<RoleDto>>.InternalError("Terjadi kesalahan saat mengambil data role: " + ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<RoleDto>>> Post([FromBody] CreateRoleDto createRoleDto)
        {
            try
            {
                var role = await _roleService.AddRoleAsync(createRoleDto);
                return CreatedAtAction(nameof(Get), new { id = role.Id },
                    ApiResponse<RoleDto>.Success("Role berhasil dibuat", role));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<RoleDto>.InternalError("Gagal membuat role: " + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Put(int id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            try
            {
                await _roleService.UpdateRoleAsync(id, updateRoleDto);
                return Ok(ApiResponse<bool>.Success("Role berhasil diperbarui", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal memperbarui role: " + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                await _roleService.DeleteRoleAsync(id);
                return Ok(ApiResponse<bool>.Success("Role berhasil dihapus", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal menghapus role: " + ex.Message));
            }
        }
    }
}
