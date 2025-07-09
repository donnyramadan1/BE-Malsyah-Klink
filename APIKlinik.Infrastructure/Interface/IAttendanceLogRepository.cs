using APIKlinik.Domain.Entities;
using APIKlinik.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Infrastructure.Interface
{
    public interface IAttendanceLogRepository : IRepository<AttendanceLog>
    {
        Task<IEnumerable<AttendanceLog>> GetByUserIdAsync(int userId, DateTime? startDate, DateTime? endDate);
        Task LogAttendanceAttemptAsync(int userId, string action, string status, string reason,
            decimal? latitude = null, decimal? longitude = null, string faceImage = null);
    }
}
