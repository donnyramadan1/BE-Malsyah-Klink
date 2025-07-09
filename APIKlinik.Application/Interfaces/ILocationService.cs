using APIKlinik.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Interfaces
{
    public interface ILocationService
    {
        Task<LocationDto> GetClinicLocationAsync();

        Task<LocationDto> AddLocationAsync(CreateLocationDto createLocationDto);

        Task UpdateLocationAsync(UpdateLocationDto updateLocationDto);
    }
}
