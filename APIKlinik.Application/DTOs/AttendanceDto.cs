using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.DTOs
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string Status { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool IsLate { get; set; }
    }

    public class CheckInOutDto
    {
        public int UserId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string FaceImage { get; set; }
    }

    public class AttendanceRequestDto
    {
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } // izin, sakit, cuti
        public string Notes { get; set; }
    }
}
