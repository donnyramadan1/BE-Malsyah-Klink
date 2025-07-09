using APIKlinik.Domain.Entities;
using APIKlinik.Infrastructure.Data;
using APIKlinik.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Infrastructure.Repositories
{
    public class ShiftRepository : BaseRepository<Shift>, IShiftRepository
    {
        public ShiftRepository(APIDbContext context) : base(context) { }

        public async Task<bool> IsShiftNameExists(string name)
        {
            return await _entities.AnyAsync(s => s.Name.ToLower() == name.ToLower());
        }
    }
}
