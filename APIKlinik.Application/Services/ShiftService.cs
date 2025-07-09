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
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ShiftService> _logger;

        public ShiftService(IShiftRepository shiftRepository, IMapper mapper, ILogger<ShiftService> logger)
        {
            _shiftRepository = shiftRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ShiftDto>> GetAllShiftsAsync()
        {
            try
            {
                var shifts = await _shiftRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<ShiftDto>>(shifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil data semua shift.");
                throw new ApplicationException("Terjadi kesalahan saat mengambil data shift.");
            }
        }

        public async Task<ShiftDto> GetShiftByIdAsync(int id)
        {
            try
            {
                var shift = await _shiftRepository.GetByIdAsync(id);
                if (shift == null)
                    throw new ApplicationException("Shift tidak ditemukan.");

                return _mapper.Map<ShiftDto>(shift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil shift dengan ID: {ShiftId}", id);
                throw new ApplicationException("Terjadi kesalahan saat mengambil detail shift.");
            }
        }

        public async Task<ShiftDto> AddShiftAsync(CreateShiftDto createShiftDto)
        {
            try
            {
                if (await _shiftRepository.IsShiftNameExists(createShiftDto.Name))
                    throw new ApplicationException("Nama shift sudah digunakan.");

                var shift = _mapper.Map<Shift>(createShiftDto);
                await _shiftRepository.AddAsync(shift);
                return _mapper.Map<ShiftDto>(shift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menambahkan shift baru.");
                throw new ApplicationException("Terjadi kesalahan saat menambahkan shift.");
            }
        }

        public async Task UpdateShiftAsync(UpdateShiftDto updateShiftDto)
        {
            try
            {
                var shift = await _shiftRepository.GetByIdAsync(updateShiftDto.Id);
                if (shift == null)
                    throw new ApplicationException("Shift tidak ditemukan untuk diperbarui.");

                if (await _shiftRepository.IsShiftNameExists(updateShiftDto.Name) && shift.Name != updateShiftDto.Name)
                    throw new ApplicationException("Nama shift sudah digunakan.");

                _mapper.Map(updateShiftDto, shift);
                await _shiftRepository.UpdateAsync(shift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memperbarui shift dengan ID: {ShiftId}", updateShiftDto.Id);
                throw;
            }
        }

        public async Task DeleteShiftAsync(int id)
        {
            try
            {
                var shift = await _shiftRepository.GetByIdAsync(id);
                if (shift == null)
                    throw new ApplicationException("Shift tidak ditemukan untuk dihapus.");

                await _shiftRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menghapus shift dengan ID: {ShiftId}", id);
                throw new ApplicationException("Terjadi kesalahan saat menghapus shift.");
            }
        }
    }
}
