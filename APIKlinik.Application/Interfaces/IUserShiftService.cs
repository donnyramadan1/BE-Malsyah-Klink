using APIKlinik.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Interfaces
{
    public interface IUserShiftService
    {
        Task<IEnumerable<UserShiftDto>> GetAllUserShiftsAsync();
        Task<UserShiftDto> GetUserShiftByIdAsync(int id);
        Task<IEnumerable<UserShiftDto>> GetUserShiftsByUserIdAsync(int userId);
        Task<UserShiftDto> AddUserShiftAsync(CreateUserShiftDto createUserShiftDto);
        Task UpdateUserShiftAsync(UpdateUserShiftDto updateUserShiftDto);
        Task DeleteUserShiftAsync(int id);
    }
}
