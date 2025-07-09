using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Domain.Entities;
using APIKlinik.Infrastructure.Interface;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LocationService> _logger;

        public LocationService(
            ILocationRepository locationRepository,
            IMapper mapper,
            ILogger<LocationService> logger)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LocationDto> GetClinicLocationAsync()
        {
            try
            {
                var location = await _locationRepository.GetClinicLocationAsync();
                return _mapper.Map<LocationDto>(location);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil lokasi klinik.");
                throw new ApplicationException("Terjadi kesalahan saat mengambil lokasi klinik.");
            }
        }

        public async Task<LocationDto> AddLocationAsync(CreateLocationDto createLocationDto)
        {
            try
            {
                // Hanya boleh ada satu lokasi klinik
                var existingLocation = await _locationRepository.GetClinicLocationAsync();
                if (existingLocation != null)
                    throw new ApplicationException("Lokasi klinik sudah ada. Gunakan update untuk mengubah.");

                var location = _mapper.Map<Location>(createLocationDto);
                await _locationRepository.AddAsync(location);
                return _mapper.Map<LocationDto>(location);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menambahkan lokasi klinik.");
                throw new ApplicationException("Terjadi kesalahan saat menambahkan lokasi klinik.");
            }
        }

        public async Task UpdateLocationAsync(UpdateLocationDto updateLocationDto)
        {
            try
            {
                var location = await _locationRepository.GetByIdAsync(updateLocationDto.Id);
                if (location == null)
                    throw new ApplicationException("Lokasi tidak ditemukan untuk diperbarui.");

                _mapper.Map(updateLocationDto, location);
                await _locationRepository.UpdateAsync(location);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memperbarui lokasi dengan ID: {LocationId}", updateLocationDto.Id);
                throw new ApplicationException("Terjadi kesalahan saat memperbarui lokasi.");
            }
        }
    }
}
