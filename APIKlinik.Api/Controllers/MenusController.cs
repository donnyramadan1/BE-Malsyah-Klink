using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIKlinik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MenusController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<MenuDto>>>> Get()
        {
            try
            {
                var menus = await _menuService.GetAllMenusAsync();
                return Ok(ApiResponse<IEnumerable<MenuDto>>.Success("Berhasil mengambil data menu", menus));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<MenuDto>>.InternalError("Terjadi kesalahan saat mengambil data menu: " + ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MenuDto>>> Get(int id)
        {
            try
            {
                var menu = await _menuService.GetMenuByIdAsync(id);
                if (menu == null)
                {
                    return NotFound(ApiResponse<MenuDto>.NotFound("Menu tidak ditemukan"));
                }

                return Ok(ApiResponse<MenuDto>.Success("Berhasil mengambil detail menu", menu));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<MenuDto>.InternalError("Terjadi kesalahan saat mengambil detail menu: " + ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<MenuDto>>> Post([FromBody] CreateMenuDto createMenuDto)
        {
            try
            {
                var menu = await _menuService.AddMenuAsync(createMenuDto);
                return CreatedAtAction(nameof(Get), new { id = menu.Id },
                    ApiResponse<MenuDto>.Success("Menu berhasil dibuat", menu));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<MenuDto>.InternalError("Gagal membuat menu: " + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Put(int id, [FromBody] UpdateMenuDto updateMenuDto)
        {
            try
            {
                await _menuService.UpdateMenuAsync(id, updateMenuDto);
                return Ok(ApiResponse<bool>.Success("Menu berhasil diperbarui", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal memperbarui menu: " + ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                await _menuService.DeleteMenuAsync(id);
                return Ok(ApiResponse<bool>.Success("Menu berhasil dihapus", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.InternalError("Gagal menghapus menu: " + ex.Message));
            }
        }
    }
}
