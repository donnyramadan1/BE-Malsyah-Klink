using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Domain.Entities;
using APIKlinik.Infrastructure.Data;
using APIKlinik.Infrastructure.Interface;
using APIKlinik.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.Linq.Expressions;

namespace APIKlinik.Application.Services
{
    public class DiagnosisService : IDiagnosisService
    {
        private readonly IDiagnosisRepository _repo;
        private readonly APIDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<DiagnosisService> _logger;

        public DiagnosisService(IDiagnosisRepository repo, APIDbContext dbContext, IMapper mapper, ILogger<DiagnosisService> logger)
        {
            _repo = repo;
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<DiagnosisDto>> GetAllAsync()
        {
            try
            {
                var data = await _repo.GetAllAsync();
                return _mapper.Map<IEnumerable<DiagnosisDto>>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil semua diagnosis");
                throw new ApplicationException("Terjadi kesalahan saat mengambil data diagnosis.");
            }
        }

        public async Task<DiagnosisDto> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) throw new ApplicationException("Diagnosis tidak ditemukan");
                return _mapper.Map<DiagnosisDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil diagnosis dengan ID: {DiagnosisId}", id);
                throw new ApplicationException("Terjadi kesalahan saat mengambil diagnosis.");
            }
        }

        public async Task<PagedResult<DiagnosisDto>> GetPagedAsync(int page, int pageSize, string? search = null)
        {
            try
            {
                Expression<Func<Diagnosis, bool>>? filter = null;

                if (!string.IsNullOrWhiteSpace(search))
                {
                    filter = d => d.Name.ToLower().Contains(search.ToLower()) ||
                                  d.Code.ToLower().Contains(search.ToLower());
                }

                var result = await _repo.GetPagedAsync(page, pageSize, filter);
                return new PagedResult<DiagnosisDto>
                {
                    Items = _mapper.Map<IEnumerable<DiagnosisDto>>(result.Items),
                    TotalItems = result.TotalItems,
                    Page = result.Page,
                    PageSize = result.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memuat diagnosis dengan pagination");
                throw new ApplicationException("Terjadi kesalahan saat mengambil data diagnosis.");
            }
        }

        public async Task<DiagnosisDto> AddAsync(CreateDiagnosisDto dto)
        {
            try
            {
                var entity = _mapper.Map<Diagnosis>(dto);
                await _repo.AddAsync(entity);
                return _mapper.Map<DiagnosisDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menambahkan diagnosis");
                throw new ApplicationException("Terjadi kesalahan saat menambahkan diagnosis.");
            }
        }

        public async Task UpdateAsync(int id, UpdateDiagnosisDto dto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) throw new ApplicationException("Diagnosis tidak ditemukan untuk diperbarui");

                _mapper.Map(dto, entity);
                await _repo.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memperbarui diagnosis dengan ID: {DiagnosisId}", id);
                throw new ApplicationException("Terjadi kesalahan saat memperbarui diagnosis.");
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null) throw new ApplicationException("Diagnosis tidak ditemukan untuk dihapus");

                await _repo.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menghapus diagnosis dengan ID: {DiagnosisId}", id);
                throw new ApplicationException("Terjadi kesalahan saat menghapus diagnosis.");
            }
        }

        public async Task<List<DiagnosisDto>> BulkUploadAsync(Stream fileStream)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage(fileStream);
                var sheet = package.Workbook.Worksheets[0];

                var allEntities = new List<Diagnosis>();
                var failedCodes = new HashSet<string>();
                var seenCodes = new HashSet<string>();

                for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                {
                    var code = sheet.Cells[row, 1].Text?.Trim();
                    var description = sheet.Cells[row, 2].Text?.Trim();

                    if (string.IsNullOrWhiteSpace(code) || !seenCodes.Add(code))
                    {
                        failedCodes.Add(code ?? $"ROW-{row}");
                        continue;
                    }

                    var name = description?.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "Tidak Diketahui";
                    name = char.ToUpper(name[0]) + name.Substring(1);

                    allEntities.Add(new Diagnosis
                    {
                        Code = code,
                        Name = name,
                        Description = description,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }

                var existingCodes = (await _repo.FindAsync(d => seenCodes.Contains(d.Code)))
                                    .Select(d => d.Code).ToHashSet();

                var toInsert = allEntities
                    .Where(d => !existingCodes.Contains(d.Code))
                    .ToList();

                var failedInsert = allEntities
                    .Where(d => existingCodes.Contains(d.Code))
                    .Select(d => d.Code);

                foreach (var code in failedInsert)
                    failedCodes.Add(code);

                await _repo.BulkInsertAsync(toInsert);

                _logger.LogInformation("Bulk upload diagnosis selesai: {Sukses} berhasil, {Gagal} gagal",
                    toInsert.Count, failedCodes.Count);

                return _mapper.Map<List<DiagnosisDto>>(toInsert);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat proses bulk upload diagnosis.");
                throw new ApplicationException("Gagal melakukan bulk upload diagnosis.");
            }
        }
    }
}
