using APIKlinik.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Interfaces
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuDto>> GetAllMenusAsync();
        Task<MenuDto> GetMenuByIdAsync(int id);
        Task<MenuDto> AddMenuAsync(CreateMenuDto createMenuDto);
        Task UpdateMenuAsync(int id, UpdateMenuDto updateMenuDto);
        Task DeleteMenuAsync(int id);
    }
}
