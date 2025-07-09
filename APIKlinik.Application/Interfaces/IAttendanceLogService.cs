using APIKlinik.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Interfaces
{
    public interface IAttendanceLogService
    {
        Task<IEnumerable<AttendanceLogDto>> GetAllAttendanceLogsAsync();

        Task<IEnumerable<AttendanceLogDto>> GetAttendanceLogsByUserIdAsync(
            int userId,
            DateTime? startDate,
            DateTime? endDate);

        Task LogAttendanceAttemptAsync(
            int userId,
            string action,
            string status,
            string reason,
            decimal? latitude = null,
            decimal? longitude = null,
            string faceImage = null);
    }
}
