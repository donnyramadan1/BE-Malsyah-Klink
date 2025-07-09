using APIKlinik.Domain.Entities;
using APIKlinik.Infrastructure.Data;
using APIKlinik.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIKlinik.Infrastructure.Repositories
{
    public class FaceDataRepository : BaseRepository<FaceData>, IFaceDataRepository
    {
        public FaceDataRepository(APIDbContext context) : base(context) { }

        public async Task<FaceData> GetByUserIdAsync(int userId)
        {
            return await _entities.FirstOrDefaultAsync(fd => fd.UserId == userId && fd.IsActive);
        }

        public async Task<bool> DeactivateAllForUserAsync(int userId)
        {
            var faceDatas = await _entities.Where(fd => fd.UserId == userId && fd.IsActive).ToListAsync();

            foreach (var faceData in faceDatas)
            {
                faceData.IsActive = false;
                faceData.UpdatedAt = DateTime.UtcNow;
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
