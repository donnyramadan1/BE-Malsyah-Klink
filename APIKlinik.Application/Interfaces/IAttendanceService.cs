using APIKlinik.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Interfaces
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceDto>> GetAllAttendancesAsync();
        Task<AttendanceDto> GetAttendanceByIdAsync(int id);
        Task<IEnumerable<AttendanceDto>> GetAttendancesByUserIdAsync(int userId, DateTime? startDate, DateTime? endDate);
        Task<AttendanceDto> CheckInAsync(CheckInOutDto checkInDto);
        Task<AttendanceDto> CheckOutAsync(CheckInOutDto checkOutDto);
        Task<AttendanceDto> SubmitAttendanceRequestAsync(AttendanceRequestDto requestDto);
    }
}
