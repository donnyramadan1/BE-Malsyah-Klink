using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.DTOs
{
    public class UserWithRolesDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }

        public List<RoleDto>? Roles { get; set; }
    }
}
