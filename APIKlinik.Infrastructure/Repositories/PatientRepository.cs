using APIKlinik.Domain.Entities;
using APIKlinik.Infrastructure.Data;
using APIKlinik.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Infrastructure.Repositories
{
    public class PatientRepository : BaseRepository<Patient>, IPatientRepository
    {
        public PatientRepository(APIDbContext context) : base(context) { }
    }
}
