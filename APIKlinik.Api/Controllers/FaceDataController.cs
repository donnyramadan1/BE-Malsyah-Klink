using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIKlinik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FaceDataController : ControllerBase
    {
        private readonly IFaceDataService _faceDataService;

        public FaceDataController(IFaceDataService faceDataService)
        {
            _faceDataService = faceDataService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<FaceDataDto>>>> GetAll()
        {
            try
            {
                var faceDatas = await _faceDataService.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<FaceDataDto>>.Success("Berhasil mengambil semua data wajah", faceDatas));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<FaceDataDto>>.InternalError("Terjadi kesalahan saat mengambil data wajah: " + ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<FaceDataDto>>> GetById(int id)
        {
            try
            {
                var faceData = await _faceDataService.GetByIdAsync(id);
                if (faceData == null)
                    return NotFound(ApiResponse<FaceDataDto>.NotFound("Data wajah tidak ditemukan"));

                return Ok(ApiResponse<FaceDataDto>.Success("Berhasil mengambil data wajah", faceData));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<FaceDataDto>.InternalError($"Terjadi kesalahan saat mengambil data wajah dengan ID {id}: " + ex.Message));
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<FaceDataDto>>> GetByUserId(int userId)
        {
            try
            {
                var faceData = await _faceDataService.GetByUserIdAsync(userId);
                if (faceData == null)
                    return NotFound(ApiResponse<FaceDataDto>.NotFound("Data wajah tidak ditemukan untuk pengguna ini"));

                return Ok(ApiResponse<FaceDataDto>.Success("Berhasil mengambil data wajah pengguna", faceData));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<FaceDataDto>.InternalError($"Terjadi kesalahan saat mengambil data wajah untuk pengguna {userId}: " + ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<FaceDataDto>>> Create([FromBody] CreateFaceDataDto createFaceDataDto)
        {
            try
            {
                var faceData = await _faceDataService.AddAsync(createFaceDataDto);
                return CreatedAtAction(nameof(GetById), new { id = faceData.Id },
                    ApiResponse<FaceDataDto>.Success("Data wajah berhasil ditambahkan", faceData));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<FaceDataDto>.InternalError("Gagal menambahkan data wajah: " + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Update(int id, [FromBody] UpdateFaceDataDto updateFaceDataDto)
        {
            try
            {
                await _faceDataService.UpdateAsync(id, updateFaceDataDto);
                return Ok(ApiResponse<bool>.Success("Data wajah berhasil diperbarui", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError($"Gagal memperbarui data wajah dengan ID {id}: " + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                await _faceDataService.DeleteAsync(id);
                return Ok(ApiResponse<bool>.Success("Data wajah berhasil dihapus", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError($"Gagal menghapus data wajah dengan ID {id}: " + ex.Message));
            }
        }
    }
}
