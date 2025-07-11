using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Domain.Entities;
using APIKlinik.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Services
{
    public class AllergyTypeService : IAllergyTypeService
    {
        private readonly IAllergyTypeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AllergyTypeService> _logger;

        public AllergyTypeService(IAllergyTypeRepository repository, IMapper mapper, ILogger<AllergyTypeService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AllergyTypeDto>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<AllergyTypeDto>>(list);
        }

        public async Task<AllergyTypeDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new ApplicationException("Jenis alergi tidak ditemukan.");
            return _mapper.Map<AllergyTypeDto>(entity);
        }

        public async Task<PagedResult<AllergyTypeDto>> GetPagedAsync(int page, int pageSize, string? search = null)
        {
            Expression<Func<AllergyType, bool>>? filter = null;

            if (!string.IsNullOrEmpty(search))
            {
                filter = x => x.Name.ToLower().Contains(search.ToLower()) ||
                              x.Code.ToLower().Contains(search.ToLower());
            }

            var result = await _repository.GetPagedAsync(page, pageSize, filter);
            return new PagedResult<AllergyTypeDto>
            {
                Items = _mapper.Map<IEnumerable<AllergyTypeDto>>(result.Items),
                TotalItems = result.TotalItems,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }

        public async Task<AllergyTypeDto> AddAsync(CreateAllergyTypeDto dto)
        {
            var entity = _mapper.Map<AllergyType>(dto);
            await _repository.AddAsync(entity);
            return _mapper.Map<AllergyTypeDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateAllergyTypeDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new ApplicationException("Jenis alergi tidak ditemukan.");

            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new ApplicationException("Jenis alergi tidak ditemukan.");

            await _repository.DeleteAsync(id);
        }
    }
}
