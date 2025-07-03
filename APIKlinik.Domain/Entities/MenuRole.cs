using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Domain.Entities
{
    public class MenuRole : BaseEntity
    {
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
