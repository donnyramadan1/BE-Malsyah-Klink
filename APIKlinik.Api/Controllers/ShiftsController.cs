using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIKlinik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShiftsController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        public ShiftsController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ShiftDto>>>> Get()
        {
            try
            {
                var shifts = await _shiftService.GetAllShiftsAsync();
                return Ok(ApiResponse<IEnumerable<ShiftDto>>.Success("Berhasil mengambil data shift", shifts));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<ShiftDto>>.InternalError("Terjadi kesalahan saat mengambil data shift: " + ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ShiftDto>>> Get(int id)
        {
            try
            {
                var shift = await _shiftService.GetShiftByIdAsync(id);
                if (shift == null)
                    return NotFound(ApiResponse<ShiftDto>.NotFound("Shift tidak ditemukan"));

                return Ok(ApiResponse<ShiftDto>.Success("Berhasil mengambil detail shift", shift));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ShiftDto>.InternalError("Terjadi kesalahan saat mengambil detail shift: " + ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ShiftDto>>> Post([FromBody] CreateShiftDto createShiftDto)
        {
            try
            {
                var shift = await _shiftService.AddShiftAsync(createShiftDto);
                return CreatedAtAction(nameof(Get), new { id = shift.Id },
                    ApiResponse<ShiftDto>.Success("Shift berhasil dibuat", shift));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ShiftDto>.InternalError("Gagal membuat shift: " + ex.Message));
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> Put([FromBody] UpdateShiftDto updateShiftDto)
        {
            try
            {
                await _shiftService.UpdateShiftAsync(updateShiftDto);
                return Ok(ApiResponse<bool>.Success("Shift berhasil diperbarui", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal memperbarui shift: " + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                await _shiftService.DeleteShiftAsync(id);
                return Ok(ApiResponse<bool>.Success("Shift berhasil dihapus", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal menghapus shift: " + ex.Message));
            }
        }
    }
}
