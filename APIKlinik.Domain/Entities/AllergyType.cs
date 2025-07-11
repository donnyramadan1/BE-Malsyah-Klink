using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Domain.Entities
{
    public class AllergyType : BaseEntity
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string? SeverityLevel { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
