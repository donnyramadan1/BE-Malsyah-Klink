using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIKlinik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AttendanceDto>>>> Get()
        {
            try
            {
                var attendances = await _attendanceService.GetAllAttendancesAsync();
                return Ok(ApiResponse<IEnumerable<AttendanceDto>>.Success("Berhasil mengambil data presensi", attendances));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<AttendanceDto>>.InternalError("Terjadi kesalahan saat mengambil data presensi: " + ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AttendanceDto>>> Get(int id)
        {
            try
            {
                var attendance = await _attendanceService.GetAttendanceByIdAsync(id);
                if (attendance == null)
                    return NotFound(ApiResponse<AttendanceDto>.NotFound("Presensi tidak ditemukan"));

                return Ok(ApiResponse<AttendanceDto>.Success("Berhasil mengambil detail presensi", attendance));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AttendanceDto>.InternalError("Terjadi kesalahan saat mengambil detail presensi: " + ex.Message));
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AttendanceDto>>>> GetByUserId(int userId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var attendances = await _attendanceService.GetAttendancesByUserIdAsync(userId, startDate, endDate);
                return Ok(ApiResponse<IEnumerable<AttendanceDto>>.Success("Berhasil mengambil presensi", attendances));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<AttendanceDto>>.InternalError("Terjadi kesalahan saat mengambil presensi: " + ex.Message));
            }
        }

        [HttpPost("checkin")]
        public async Task<ActionResult<ApiResponse<AttendanceDto>>> CheckIn([FromBody] CheckInOutDto checkInDto)
        {
            try
            {
                var attendance = await _attendanceService.CheckInAsync(checkInDto);
                return CreatedAtAction(nameof(Get), new { id = attendance.Id },
                    ApiResponse<AttendanceDto>.Success("Check-in berhasil", attendance));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AttendanceDto>.InternalError("Gagal melakukan check-in: " + ex.Message));
            }
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<ApiResponse<AttendanceDto>>> CheckOut([FromBody] CheckInOutDto checkOutDto)
        {
            try
            {
                var attendance = await _attendanceService.CheckOutAsync(checkOutDto);
                return Ok(ApiResponse<AttendanceDto>.Success("Check-out berhasil", attendance));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AttendanceDto>.InternalError("Gagal melakukan check-out: " + ex.Message));
            }
        }

        [HttpPost("request")]
        public async Task<ActionResult<ApiResponse<AttendanceDto>>> SubmitRequest([FromBody] AttendanceRequestDto requestDto)
        {
            try
            {
                var attendance = await _attendanceService.SubmitAttendanceRequestAsync(requestDto);
                return CreatedAtAction(nameof(Get), new { id = attendance.Id },
                    ApiResponse<AttendanceDto>.Success("Permohonan presensi berhasil diajukan", attendance));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AttendanceDto>.InternalError("Gagal mengajukan permohonan presensi: " + ex.Message));
            }
        }
    }
}
