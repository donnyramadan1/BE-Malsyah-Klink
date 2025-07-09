using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIKlinik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendanceLogsController : ControllerBase
    {
        private readonly IAttendanceLogService _attendanceLogService;

        public AttendanceLogsController(IAttendanceLogService attendanceLogService)
        {
            _attendanceLogService = attendanceLogService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AttendanceLogDto>>>> Get()
        {
            try
            {
                var logs = await _attendanceLogService.GetAllAttendanceLogsAsync();
                return Ok(ApiResponse<IEnumerable<AttendanceLogDto>>.Success("Berhasil mengambil data log presensi", logs));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<AttendanceLogDto>>.InternalError("Terjadi kesalahan saat mengambil data log presensi: " + ex.Message));
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AttendanceLogDto>>>> GetByUserId(int userId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var logs = await _attendanceLogService.GetAttendanceLogsByUserIdAsync(userId, startDate, endDate);
                return Ok(ApiResponse<IEnumerable<AttendanceLogDto>>.Success("Berhasil mengambil log presensi", logs));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<AttendanceLogDto>>.InternalError("Terjadi kesalahan saat mengambil log presensi: " + ex.Message));
            }
        }
    }
}

