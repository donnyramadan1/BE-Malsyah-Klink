using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIKlinik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserShiftsController : ControllerBase
    {
        private readonly IUserShiftService _userShiftService;

        public UserShiftsController(IUserShiftService userShiftService)
        {
            _userShiftService = userShiftService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserShiftDto>>>> Get()
        {
            try
            {
                var userShifts = await _userShiftService.GetAllUserShiftsAsync();
                return Ok(ApiResponse<IEnumerable<UserShiftDto>>.Success("Berhasil mengambil data user shift", userShifts));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<UserShiftDto>>.InternalError("Terjadi kesalahan saat mengambil data user shift: " + ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserShiftDto>>> Get(int id)
        {
            try
            {
                var userShift = await _userShiftService.GetUserShiftByIdAsync(id);
                if (userShift == null)
                    return NotFound(ApiResponse<UserShiftDto>.NotFound("User shift tidak ditemukan"));

                return Ok(ApiResponse<UserShiftDto>.Success("Berhasil mengambil detail user shift", userShift));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserShiftDto>.InternalError("Terjadi kesalahan saat mengambil detail user shift: " + ex.Message));
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserShiftDto>>>> GetByUserId(int userId)
        {
            try
            {
                var userShifts = await _userShiftService.GetUserShiftsByUserIdAsync(userId);
                return Ok(ApiResponse<IEnumerable<UserShiftDto>>.Success("Berhasil mengambil user shift", userShifts));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<UserShiftDto>>.InternalError("Terjadi kesalahan saat mengambil user shift: " + ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserShiftDto>>> Post([FromBody] CreateUserShiftDto createUserShiftDto)
        {
            try
            {
                var userShift = await _userShiftService.AddUserShiftAsync(createUserShiftDto);
                return CreatedAtAction(nameof(Get), new { id = userShift.Id },
                    ApiResponse<UserShiftDto>.Success("User shift berhasil dibuat", userShift));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserShiftDto>.InternalError("Gagal membuat user shift: " + ex.Message));
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> Put([FromBody] UpdateUserShiftDto updateUserShiftDto)
        {
            try
            {
                await _userShiftService.UpdateUserShiftAsync(updateUserShiftDto);
                return Ok(ApiResponse<bool>.Success("User shift berhasil diperbarui", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal memperbarui user shift: " + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                await _userShiftService.DeleteUserShiftAsync(id);
                return Ok(ApiResponse<bool>.Success("User shift berhasil dihapus", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal menghapus user shift: " + ex.Message));
            }
        }
    }
}
