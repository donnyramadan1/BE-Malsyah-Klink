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
    public class DiagnosesController : ControllerBase
    {
        private readonly IDiagnosisService _service;
        private readonly ILogger<DiagnosesController> _logger;

        public DiagnosesController(IDiagnosisService service, ILogger<DiagnosesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<DiagnosisDto>>>> Get()
        {
            try
            {
                var data = await _service.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<DiagnosisDto>>.Success("Berhasil mengambil data diagnosis", data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil data diagnosis");
                return StatusCode(500, ApiResponse<string>.InternalError("Terjadi kesalahan saat mengambil data diagnosis: " + ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DiagnosisDto>>> Get(int id)
        {
            try
            {
                var data = await _service.GetByIdAsync(id);
                return Ok(ApiResponse<DiagnosisDto>.Success("Berhasil mengambil detail diagnosis", data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil diagnosis dengan ID: {DiagnosisId}", id);
                return StatusCode(500, ApiResponse<string>.InternalError("Terjadi kesalahan saat mengambil detail diagnosis: " + ex.Message));
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponse<PagedResult<DiagnosisDto>>>> GetPaged(int page = 1, int pageSize = 10, string? search = null)
        {
            try
            {
                var result = await _service.GetPagedAsync(page, pageSize, search);
                return Ok(ApiResponse<PagedResult<DiagnosisDto>>.Success("Berhasil mengambil data diagnosis dengan pagination", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memuat data diagnosis paged");
                return StatusCode(500, ApiResponse<string>.InternalError("Terjadi kesalahan saat memuat data diagnosis: " + ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<DiagnosisDto>>> Post([FromBody] CreateDiagnosisDto dto)
        {
            try
            {
                var result = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, ApiResponse<DiagnosisDto>.Success("Diagnosis berhasil ditambahkan", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menambahkan diagnosis");
                return StatusCode(500, ApiResponse<string>.InternalError("Terjadi kesalahan saat menambahkan diagnosis: " + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Put(int id, [FromBody] UpdateDiagnosisDto dto)
        {
            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok(ApiResponse<bool>.Success("Diagnosis berhasil diperbarui", true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memperbarui diagnosis ID: {DiagnosisId}", id);
                return StatusCode(500, ApiResponse<string>.InternalError("Terjadi kesalahan saat memperbarui diagnosis: " + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(ApiResponse<bool>.Success("Diagnosis berhasil dihapus", true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menghapus diagnosis ID: {DiagnosisId}", id);
                return StatusCode(500, ApiResponse<string>.InternalError("Terjadi kesalahan saat menghapus diagnosis: " + ex.Message));
            }
        }

        [HttpPost("upload")]
        public async Task<ActionResult<ApiResponse<object>>> BulkUpload([FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(ApiResponse<string>.BadRequest("File tidak valid"));

                using var stream = file.OpenReadStream();
                var uploaded = await _service.BulkUploadAsync(stream);

                return Ok(ApiResponse<object>.Success("Upload diagnosis selesai", new
                {
                    Sukses = uploaded.Count,
                    Gagal = "Duplikat atau sudah ada. Cek log untuk detail."
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal melakukan upload file diagnosis");
                return StatusCode(500, ApiResponse<string>.InternalError("Terjadi kesalahan saat upload diagnosis: " + ex.Message));
            }
        }
    }
}
