using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Domain.Entities
{
    public class Role : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
   
        public ICollection<UserRole>? UserRoles { get; set; }
        public ICollection<MenuRole>? MenuRoles { get; set; }
    }
}
