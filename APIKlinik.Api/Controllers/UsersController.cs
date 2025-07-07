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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> Get()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(ApiResponse<IEnumerable<UserDto>>.Success("Berhasil mengambil data user", users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<UserDto>>.InternalError("Terjadi kesalahan saat mengambil data user: " + ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Get(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound(ApiResponse<UserDto>.NotFound("User tidak ditemukan"));

                return Ok(ApiResponse<UserDto>.Success("Berhasil mengambil detail user", user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserDto>.InternalError("Terjadi kesalahan saat mengambil detail user: " + ex.Message));
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponse<PagedResult<UserDto>>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _userService.GetPagedUsersAsync(page, pageSize);
                return Ok(ApiResponse<PagedResult<UserDto>>.Success("Berhasil mengambil data user role dengan pagination", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PagedResult<UserDto>>.InternalError("Terjadi kesalahan saat mengambil data user role: " + ex.Message));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<UserDto>>> Post([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                var user = await _userService.AddUserAsync(createUserDto);
                return CreatedAtAction(nameof(Get), new { id = user.Id },
                    ApiResponse<UserDto>.Success("User berhasil dibuat", user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserDto>.InternalError("Gagal membuat user: " + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Put(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                await _userService.UpdateUserAsync(id, updateUserDto);
                return Ok(ApiResponse<bool>.Success("User berhasil diperbarui", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal memperbarui user: " + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok(ApiResponse<bool>.Success("User berhasil dihapus", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal menghapus user: " + ex.Message));
            }
        }
    }
}
