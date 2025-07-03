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
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(APIDbContext context) : base(context) { }
    }

    public interface IUserRepository : IRepository<User> { }
}
