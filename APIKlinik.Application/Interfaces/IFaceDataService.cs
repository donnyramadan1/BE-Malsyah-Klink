using APIKlinik.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Interfaces
{
    public interface IFaceDataService
    {
        Task<IEnumerable<FaceDataDto>> GetAllAsync();
        Task<FaceDataDto> GetByIdAsync(int id);
        Task<FaceDataDto> GetByUserIdAsync(int userId);
        Task<FaceDataDto> AddAsync(CreateFaceDataDto createFaceDataDto);
        Task UpdateAsync(int id, UpdateFaceDataDto updateFaceDataDto);
        Task DeleteAsync(int id);
        Task DeactivateAllForUserAsync(int userId);
    }
}
