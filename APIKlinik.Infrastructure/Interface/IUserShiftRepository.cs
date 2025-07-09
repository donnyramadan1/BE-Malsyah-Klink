using APIKlinik.Domain.Entities;
using APIKlinik.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Infrastructure.Interface
{
    public interface IUserShiftRepository : IRepository<UserShift>
    {
        Task<IEnumerable<UserShift>> GetByUserIdAsync(int userId);
        Task<UserShift> GetByUserAndDateAsync(int userId, DateTime date);
        Task<bool> IsUserShiftExists(int userId, DateTime? date);
    }
}
