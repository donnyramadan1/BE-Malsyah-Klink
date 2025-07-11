using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Domain.Entities;
using APIKlinik.Infrastructure.Interface;
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
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<PatientService> _logger;

        public PatientService(IPatientRepository repo, IMapper mapper, ILogger<PatientService> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            try
            {
                var data = await _repo.GetAllAsync();
                return _mapper.Map<IEnumerable<PatientDto>>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil semua pasien.");
                throw new ApplicationException("Gagal mengambil data pasien.");
            }
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            try
            {
                var data = await _repo.GetByIdAsync(id);
                return _mapper.Map<PatientDto>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil detail pasien.");
                throw new ApplicationException("Gagal mengambil detail pasien.");
            }
        }

        public async Task<PagedResult<PatientDto>> GetPagedAsync(int page, int pageSize, string? search = null)
        {
            try
            {
                Expression<Func<Patient, bool>>? filter = null;
                if (!string.IsNullOrEmpty(search))
                    filter = p => p.Name.ToLower().Contains(search.ToLower()) || (p.MedicalRecordNumber != null && p.MedicalRecordNumber.ToLower().Contains(search.ToLower()));

                var result = await _repo.GetPagedAsync(page, pageSize, filter);
                return new PagedResult<PatientDto>
                {
                    Items = _mapper.Map<IEnumerable<PatientDto>>(result.Items),
                    TotalItems = result.TotalItems,
                    Page = result.Page,
                    PageSize = result.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil data pasien secara paged.");
                throw new ApplicationException("Gagal mengambil data pasien.");
            }
        }

        public async Task<PatientDto> AddAsync(CreatePatientDto dto)
        {
            try
            {
                // 1. Buat prefix MK-YYYYMMDD
                var today = DateTime.UtcNow;
                var prefix = $"MK-{today:yyyyMMdd}";

                // 2. Cari jumlah pasien hari ini (menggunakan medical record yang cocok prefix)
                var existingToday = await _repo.FindAsync(p =>
                    p.MedicalRecordNumber.StartsWith(prefix));

                // 3. Tentukan urutan terakhir hari ini
                int lastNumber = 0;

                if (existingToday.Any())
                {
                    lastNumber = existingToday
                        .Select(p => p.MedicalRecordNumber)
                        .Select(code =>
                        {
                            var parts = code.Split('-');
                            return int.TryParse(parts.Last(), out int num) ? num : 0;
                        })
                        .Max();
                }

                var nextNumber = lastNumber + 1;
                var formattedNumber = nextNumber.ToString("D3"); // 3 digit

                // 4. Generate MedicalRecordNumber final
                var mrn = $"{prefix}-{formattedNumber}";

                // 5. Map dan assign MRN
                var entity = _mapper.Map<Patient>(dto);
                entity.MedicalRecordNumber = mrn;

                await _repo.AddAsync(entity);

                return _mapper.Map<PatientDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menambahkan pasien.");
                throw new ApplicationException("Gagal menambahkan pasien.");
            }
        }


        public async Task UpdateAsync(int id, UpdatePatientDto dto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) throw new ApplicationException("Pasien tidak ditemukan.");
                _mapper.Map(dto, entity);
                await _repo.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memperbarui pasien.");
                throw new ApplicationException("Gagal memperbarui pasien.");
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _repo.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menghapus pasien.");
                throw new ApplicationException("Gagal menghapus pasien.");
            }
        }
    }
}
