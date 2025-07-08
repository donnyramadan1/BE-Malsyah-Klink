using APIKlinik.Application.DTOs;
using APIKlinik.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<PagedResult<UserDto>> GetPagedUsersAsync(int page, int pageSize, string? search = null);
        Task<UserDto> AddUserAsync(CreateUserDto createUserDto);
        Task UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task DeleteUserAsync(int id);
    }
}
