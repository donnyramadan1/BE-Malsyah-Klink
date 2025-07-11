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
    public class AllergyTypesController : ControllerBase
    {
        private readonly IAllergyTypeService _service;

        public AllergyTypesController(IAllergyTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AllergyTypeDto>>>> Get()
        {
            var data = await _service.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<AllergyTypeDto>>.Success("Berhasil mengambil data alergi", data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AllergyTypeDto>>> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);
            return Ok(ApiResponse<AllergyTypeDto>.Success("Berhasil mengambil detail alergi", data));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<ApiResponse<PagedResult<AllergyTypeDto>>>> GetPaged(int page = 1, int pageSize = 10, string? search = null)
        {
            var result = await _service.GetPagedAsync(page, pageSize, search);
            return Ok(ApiResponse<PagedResult<AllergyTypeDto>>.Success("Berhasil mengambil data alergi dengan pagination", result));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<AllergyTypeDto>>> Post([FromBody] CreateAllergyTypeDto dto)
        {
            var result = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, ApiResponse<AllergyTypeDto>.Success("Jenis alergi berhasil ditambahkan", result));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Put(int id, [FromBody] UpdateAllergyTypeDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(ApiResponse<bool>.Success("Jenis alergi berhasil diperbarui", true));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse<bool>.Success("Jenis alergi berhasil dihapus", true));
        }
    }
}
