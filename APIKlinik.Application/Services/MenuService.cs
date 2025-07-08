using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Domain.Entities;
using APIKlinik.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace APIKlinik.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MenuService> _logger;

        public MenuService(IRepository<Menu> menuRepository, IMapper mapper, ILogger<MenuService> logger)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<MenuDto>> GetAllMenusAsync()
        {
            try
            {
                var menus = await _menuRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<MenuDto>>(menus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil semua data menu.");
                throw new ApplicationException("Terjadi kesalahan saat mengambil data menu.");
            }
        }

        public async Task<MenuDto> GetMenuByIdAsync(int id)
        {
            try
            {
                var menu = await _menuRepository.GetByIdAsync(id);
                if (menu == null)
                    throw new ApplicationException("Menu tidak ditemukan.");

                return _mapper.Map<MenuDto>(menu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil menu dengan ID: {MenuId}", id);
                throw new ApplicationException("Terjadi kesalahan saat mengambil detail menu.");
            }
        }

        public async Task<PagedResult<MenuDto>> GetPagedMenusAsync(int page, int pageSize, string? search = null)
        {
            try
            {
                Expression<Func<Menu, bool>>? filter = null;

                if (!string.IsNullOrEmpty(search))
                {
                    filter = m => m.Title.Contains(search) ||
                                 (m.Path != null && m.Path.Contains(search));                                 
                }

                var result = await _menuRepository.GetPagedOrderedAsync(page, pageSize, filter, m => m.OrderNum);
                return new PagedResult<MenuDto>
                {
                    Items = _mapper.Map<IEnumerable<MenuDto>>(result.Items),
                    TotalItems = result.TotalItems,
                    Page = result.Page,
                    PageSize = result.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memuat data menu dengan pagination.");
                throw new ApplicationException("Terjadi kesalahan saat memuat data menu.");
            }
        }

        public async Task<MenuDto> AddMenuAsync(CreateMenuDto createMenuDto)
        {
            try
            {
                var menu = _mapper.Map<Menu>(createMenuDto);
                await _menuRepository.AddAsync(menu);
                return _mapper.Map<MenuDto>(menu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menambahkan menu baru.");
                throw new ApplicationException("Terjadi kesalahan saat menambahkan menu.");
            }
        }

        public async Task UpdateMenuAsync(int id, UpdateMenuDto updateMenuDto)
        {
            try
            {
                var menu = await _menuRepository.GetByIdAsync(id);
                if (menu == null)
                    throw new ApplicationException("Menu tidak ditemukan untuk diperbarui.");

                _mapper.Map(updateMenuDto, menu);
                await _menuRepository.UpdateAsync(menu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memperbarui menu dengan ID: {MenuId}", id);
                throw new ApplicationException("Terjadi kesalahan saat memperbarui menu.");
            }
        }

        public async Task DeleteMenuAsync(int id)
        {
            try
            {
                var menu = await _menuRepository.GetByIdAsync(id);
                if (menu == null)
                    throw new ApplicationException("Menu tidak ditemukan untuk dihapus.");

                await _menuRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menghapus menu dengan ID: {MenuId}", id);
                throw new ApplicationException("Terjadi kesalahan saat menghapus menu.");
            }
        }
    }
}
