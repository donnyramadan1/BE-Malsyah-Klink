using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Domain.Entities
{
    public class Patient : BaseEntity
    {
        public int Id { get; set; }
        public string? MedicalRecordNumber { get; set; }
        public string? IdentityNumber { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? BloodType { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Allergies { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
