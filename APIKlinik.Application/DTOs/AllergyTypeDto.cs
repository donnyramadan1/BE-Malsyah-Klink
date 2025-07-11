using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.DTOs
{
    public class AllergyTypeDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? SeverityLevel { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateAllergyTypeDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? SeverityLevel { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateAllergyTypeDto
    {
        public string Name { get; set; }
        public string? SeverityLevel { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
