using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.DTOs
{
    public class DiagnosisDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class CreateDiagnosisDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateDiagnosisDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
