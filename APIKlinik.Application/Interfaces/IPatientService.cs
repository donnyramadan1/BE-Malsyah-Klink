using APIKlinik.Application.DTOs;
using APIKlinik.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllAsync();
        Task<PatientDto?> GetByIdAsync(int id);
        Task<PagedResult<PatientDto>> GetPagedAsync(int page, int pageSize, string? search = null);
        Task<PatientDto> AddAsync(CreatePatientDto dto);
        Task UpdateAsync(int id, UpdatePatientDto dto);
        Task DeleteAsync(int id);
    }
}
