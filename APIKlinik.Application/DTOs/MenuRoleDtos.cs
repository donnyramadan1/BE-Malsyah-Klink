using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.DTOs
{
    public class MenuRoleDto
    {
        public int MenuId { get; set; }
        public int RoleId { get; set; }
        public MenuDto Menu { get; set; }
        public RoleDto Role { get; set; }
    }

    public class AssignMenuRoleDto
    {
        public int MenuId { get; set; }
        public int RoleId { get; set; }
    }
}
