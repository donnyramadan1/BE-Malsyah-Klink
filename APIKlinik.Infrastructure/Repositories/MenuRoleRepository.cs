using APIKlinik.Domain.Entities;
using APIKlinik.Domain.Interfaces;
using APIKlinik.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Infrastructure.Repositories
{
    public class MenuRoleRepository : BaseRepository<MenuRole>, IMenuRoleRepository
    {
        public MenuRoleRepository(APIDbContext context) : base(context) { }
    }

    public interface IMenuRoleRepository : IRepository<MenuRole> { }
}
