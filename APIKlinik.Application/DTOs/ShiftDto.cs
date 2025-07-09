using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.DTOs
{
    public class ShiftDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int LateToleranceMinutes { get; set; }
    }

    public class CreateShiftDto
    {
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int LateToleranceMinutes { get; set; } = 15;
    }

    public class UpdateShiftDto : CreateShiftDto
    {
        public int Id { get; set; }
    }
}
