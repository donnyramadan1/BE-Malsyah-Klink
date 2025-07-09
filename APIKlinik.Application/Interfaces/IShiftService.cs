using APIKlinik.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Interfaces
{
    public interface IShiftService
    {
        Task<IEnumerable<ShiftDto>> GetAllShiftsAsync();
        Task<ShiftDto> GetShiftByIdAsync(int id);
        Task<ShiftDto> AddShiftAsync(CreateShiftDto createShiftDto);
        Task UpdateShiftAsync(UpdateShiftDto updateShiftDto);
        Task DeleteShiftAsync(int id);
    }
}
