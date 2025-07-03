using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Domain.Entities
{
    public class Menu : BaseEntity
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public int OrderNum { get; set; }
        public bool IsActive { get; set; }      

        public Menu Parent { get; set; }
        public ICollection<Menu> Children { get; set; }
        public ICollection<MenuRole> MenuRoles { get; set; }
    }
}
