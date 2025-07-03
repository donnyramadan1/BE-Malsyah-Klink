using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Domain.Entities;
using APIKlinik.Domain.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Services
{    
    public class MenuService : IMenuService
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IMapper _mapper;

        public MenuService(IRepository<Menu> menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MenuDto>> GetAllMenusAsync()
        {
            var menus = await _menuRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MenuDto>>(menus);
        }

        public async Task<MenuDto> GetMenuByIdAsync(int id)
        {
            var menu = await _menuRepository.GetByIdAsync(id);
            return _mapper.Map<MenuDto>(menu);
        }

        public async Task<MenuDto> AddMenuAsync(CreateMenuDto createMenuDto)
        {
            var menu = _mapper.Map<Menu>(createMenuDto);
            await _menuRepository.AddAsync(menu);
            return _mapper.Map<MenuDto>(menu);
        }

        public async Task UpdateMenuAsync(int id, UpdateMenuDto updateMenuDto)
        {
            var menu = await _menuRepository.GetByIdAsync(id);
            if (menu != null)
            {
                _mapper.Map(updateMenuDto, menu);
                await _menuRepository.UpdateAsync(menu);
            }
        }

        public async Task DeleteMenuAsync(int id)
        {
            await _menuRepository.DeleteAsync(id);
        }
    }
}
