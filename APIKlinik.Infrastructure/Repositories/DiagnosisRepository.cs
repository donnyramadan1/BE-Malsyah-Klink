using APIKlinik.Domain.Entities;
using APIKlinik.Domain.Interfaces;
using APIKlinik.Infrastructure.Data;
using APIKlinik.Infrastructure.Interface;
using EFCore.BulkExtensions;

namespace APIKlinik.Infrastructure.Repositories
{
    public class DiagnosisRepository : BaseRepository<Diagnosis>, IDiagnosisRepository
    {
        private readonly APIDbContext _context;

        public DiagnosisRepository(APIDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task BulkInsertAsync(List<Diagnosis> diagnoses)
        {
            await _context.BulkInsertAsync(diagnoses);
        }
    }
}
