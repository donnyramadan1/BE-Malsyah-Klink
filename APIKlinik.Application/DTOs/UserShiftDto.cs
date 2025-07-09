using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.DTOs
{
    public class UserShiftDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public DateTime? Date { get; set; }
    }

    public class CreateUserShiftDto
    {
        public int UserId { get; set; }
        public int ShiftId { get; set; }
        public DateTime? Date { get; set; }
    }

    public class UpdateUserShiftDto : CreateUserShiftDto
    {
        public int Id { get; set; }
    }
}
