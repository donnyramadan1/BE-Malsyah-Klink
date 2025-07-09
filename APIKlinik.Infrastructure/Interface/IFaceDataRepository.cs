using APIKlinik.Domain.Entities;
using APIKlinik.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Infrastructure.Interface
{
    public interface IFaceDataRepository : IRepository<FaceData>
    {
        Task<FaceData> GetByUserIdAsync(int userId);
        Task<bool> DeactivateAllForUserAsync(int userId);
    }
}
