using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIKlinik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientsController(IPatientService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PatientDto>>>> GetAll()
        {
            try
            {
                var data = await _service.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<PatientDto>>.Success("Berhasil mengambil semua pasien", data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<PatientDto>>.InternalError("Gagal: " + ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PatientDto>>> GetById(int id)
        {
            try
            {
                var data = await _service.GetByIdAsync(id);
                if (data == null)
                    return NotFound(ApiResponse<PatientDto>.NotFound("Pasien tidak ditemukan"));

                return Ok(ApiResponse<PatientDto>.Success("Berhasil ambil detail pasien", data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PatientDto>.InternalError("Gagal: " + ex.Message));
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponse<PagedResult<PatientDto>>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            try
            {
                var result = await _service.GetPagedAsync(page, pageSize, search);
                return Ok(ApiResponse<PagedResult<PatientDto>>.Success("Berhasil mengambil pasien secara paged", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PagedResult<PatientDto>>.InternalError("Gagal: " + ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PatientDto>>> Post([FromBody] CreatePatientDto dto)
        {
            try
            {
                var data = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = data.Id }, ApiResponse<PatientDto>.Success("Berhasil tambah pasien", data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PatientDto>.InternalError("Gagal: " + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Put(int id, [FromBody] UpdatePatientDto dto)
        {
            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok(ApiResponse<bool>.Success("Berhasil update pasien", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal: " + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(ApiResponse<bool>.Success("Berhasil hapus pasien", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal: " + ex.Message));
            }
        }
    }
}
