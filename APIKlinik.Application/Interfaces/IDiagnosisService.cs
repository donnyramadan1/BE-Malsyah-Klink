using APIKlinik.Application.DTOs;
using APIKlinik.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Interfaces
{
    public interface IDiagnosisService
    {
        Task<IEnumerable<DiagnosisDto>> GetAllAsync();
        Task<DiagnosisDto> GetByIdAsync(int id);
        Task<PagedResult<DiagnosisDto>> GetPagedAsync(int page, int pageSize, string? search = null);
        Task<DiagnosisDto> AddAsync(CreateDiagnosisDto dto);
        Task UpdateAsync(int id, UpdateDiagnosisDto dto);
        Task DeleteAsync(int id);
        Task<List<DiagnosisDto>> BulkUploadAsync(Stream fileStream);
    }
}
