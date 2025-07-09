using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Domain.Entities
{
    public class Attendance : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public string Status { get; set; } // hadir, izin, sakit, cuti
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string FaceImage { get; set; } // base64 atau path file
        public bool IsLate { get; set; }

        public virtual User? User { get; set; }
    }
}
