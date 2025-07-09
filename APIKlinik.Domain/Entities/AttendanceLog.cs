using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Domain.Entities
{
    public class AttendanceLog : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; } // checkin, checkout, izin, sakit
        public string Status { get; set; } // success, failed
        public string Reason { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string FaceImage { get; set; }

        public virtual User? User { get; set; }
    }
}
