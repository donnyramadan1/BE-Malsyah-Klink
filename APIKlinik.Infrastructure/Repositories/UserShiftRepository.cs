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
    public class UserShiftRepository : BaseRepository<UserShift>, IUserShiftRepository
    {
        public UserShiftRepository(APIDbContext context) : base(context) { }

        public async Task<IEnumerable<UserShift>> GetByUserIdAsync(int userId)
        {
            return await _entities
                .Include(us => us.User)
                .Include(us => us.Shift)
                .Where(us => us.UserId == userId)
                .ToListAsync();
        }

        public async Task<UserShift> GetByUserAndDateAsync(int userId, DateTime date)
        {
            return await _entities
                .Include(us => us.Shift)
                .FirstOrDefaultAsync(us => us.UserId == userId &&
                    (us.Date == null || us.Date.Value.Date == date.Date));
        }

        public async Task<bool> IsUserShiftExists(int userId, DateTime? date)
        {
            return await _entities.AnyAsync(us =>
                us.UserId == userId &&
                (us.Date == date || (date == null && us.Date == null)));
        }
    }
}
