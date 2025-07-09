using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIKlinik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<LocationDto>>> Get()
        {
            try
            {
                var location = await _locationService.GetClinicLocationAsync();
                if (location == null)
                    return NotFound(ApiResponse<LocationDto>.NotFound("Lokasi klinik belum diatur"));

                return Ok(ApiResponse<LocationDto>.Success("Berhasil mengambil lokasi klinik", location));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<LocationDto>.InternalError("Terjadi kesalahan saat mengambil lokasi klinik: " + ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<LocationDto>>> Post([FromBody] CreateLocationDto createLocationDto)
        {
            try
            {
                var location = await _locationService.AddLocationAsync(createLocationDto);
                return CreatedAtAction(nameof(Get), new { id = location.Id },
                    ApiResponse<LocationDto>.Success("Lokasi klinik berhasil dibuat", location));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<LocationDto>.InternalError("Gagal membuat lokasi klinik: " + ex.Message));
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> Put([FromBody] UpdateLocationDto updateLocationDto)
        {
            try
            {
                await _locationService.UpdateLocationAsync(updateLocationDto);
                return Ok(ApiResponse<bool>.Success("Lokasi klinik berhasil diperbarui", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal memperbarui lokasi klinik: " + ex.Message));
            }
        }
    }
}
