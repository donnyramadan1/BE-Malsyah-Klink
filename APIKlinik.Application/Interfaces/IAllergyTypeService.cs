using APIKlinik.Application.DTOs;
using APIKlinik.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Interfaces
{
    public interface IAllergyTypeService
    {
        Task<IEnumerable<AllergyTypeDto>> GetAllAsync();
        Task<AllergyTypeDto> GetByIdAsync(int id);
        Task<PagedResult<AllergyTypeDto>> GetPagedAsync(int page, int pageSize, string? search = null);
        Task<AllergyTypeDto> AddAsync(CreateAllergyTypeDto dto);
        Task UpdateAsync(int id, UpdateAllergyTypeDto dto);
        Task DeleteAsync(int id);
    }
}
