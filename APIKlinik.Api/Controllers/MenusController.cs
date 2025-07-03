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
            var menus = await _menuService.GetAllMenusAsync();
            return Ok(ApiResponse<IEnumerable<MenuDto>>.Success(menus));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MenuDto>>> Get(int id)
        {
            var menu = await _menuService.GetMenuByIdAsync(id);
            if (menu == null)
            {
                return NotFound(ApiResponse<MenuDto>.NotFound("Menu not found"));
            }
            return Ok(ApiResponse<MenuDto>.Success(menu));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<MenuDto>>> Post([FromBody] CreateMenuDto createMenuDto)
        {
            var menu = await _menuService.AddMenuAsync(createMenuDto);
            return CreatedAtAction(nameof(Get), new { id = menu.Id },
                ApiResponse<MenuDto>.Success("Menu created successfully", menu));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Put(int id, [FromBody] UpdateMenuDto updateMenuDto)
        {
            await _menuService.UpdateMenuAsync(id, updateMenuDto);
            return Ok(ApiResponse<bool>.Success("Menu updated successfully", true));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            await _menuService.DeleteMenuAsync(id);
            return Ok(ApiResponse<bool>.Success("Menu deleted successfully", true));
        }
    }
}
