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
    public class AttendanceRepository : BaseRepository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(APIDbContext context) : base(context) { }

        public async Task<Attendance> GetByUserAndDateAsync(int userId, DateTime date)
        {
            return await _entities
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.UserId == userId && a.Date.Date == date.Date);
        }

        public async Task<IEnumerable<Attendance>> GetByUserIdAsync(int userId, DateTime? startDate, DateTime? endDate)
        {
            var query = _entities
                .Include(a => a.User)
                .Where(a => a.UserId == userId);

            if (startDate.HasValue)
                query = query.Where(a => a.Date >= startDate.Value.Date);

            if (endDate.HasValue)
                query = query.Where(a => a.Date <= endDate.Value.Date);

            return await query.ToListAsync();
        }

        public async Task<bool> HasCheckedInTodayAsync(int userId, DateTime date)
        {
            return await _entities.AnyAsync(a =>
                a.UserId == userId &&
                a.Date.Date == date.Date &&
                a.CheckinTime != null);
        }

        public async Task<bool> HasCheckedOutTodayAsync(int userId, DateTime date)
        {
            return await _entities.AnyAsync(a =>
                a.UserId == userId &&
                a.Date.Date == date.Date &&
                a.CheckoutTime != null);
        }
    }
}
