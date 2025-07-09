using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Domain.Entities
{
    public class UserShift : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ShiftId { get; set; }
        public Shift Shift { get; set; }
        public DateTime? Date { get; set; } // Null untuk shift tetap
    }
}
