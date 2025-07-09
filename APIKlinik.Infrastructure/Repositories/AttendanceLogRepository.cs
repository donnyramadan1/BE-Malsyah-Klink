using APIKlinik.Domain.Entities;
using APIKlinik.Infrastructure.Data;
using APIKlinik.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Infrastructure.Repositories
{
    public class AttendanceLogRepository : BaseRepository<AttendanceLog>, IAttendanceLogRepository
    {
        public AttendanceLogRepository(APIDbContext context) : base(context) { }

        public async Task<IEnumerable<AttendanceLog>> GetByUserIdAsync(int userId, DateTime? startDate, DateTime? endDate)
        {
            var query = _entities
                .Include(al => al.User)
                .Where(al => al.UserId == userId);

            if (startDate.HasValue)
                query = query.Where(al => al.CreatedAt >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(al => al.CreatedAt <= endDate.Value);

            return await query.ToListAsync();
        }

        public async Task LogAttendanceAttemptAsync(int userId, string action, string status, string reason,
            decimal? latitude = null, decimal? longitude = null, string faceImage = null)
        {
            var log = new AttendanceLog
            {
                UserId = userId,
                Action = action,
                Status = status,
                Reason = reason,
                Latitude = latitude,
                Longitude = longitude,
                FaceImage = faceImage
            };

            await _entities.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
