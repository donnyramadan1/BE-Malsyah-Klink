using APIKlinik.Domain.Entities;
using APIKlinik.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Infrastructure.Interface
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        Task<Attendance> GetByUserAndDateAsync(int userId, DateTime date);
        Task<IEnumerable<Attendance>> GetByUserIdAsync(int userId, DateTime? startDate, DateTime? endDate);
        Task<bool> HasCheckedInTodayAsync(int userId, DateTime date);
        Task<bool> HasCheckedOutTodayAsync(int userId, DateTime date);
    }
}
