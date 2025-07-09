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
    public class FaceDataService : IFaceDataService
    {
        private readonly IFaceDataRepository _faceDataRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FaceDataService> _logger;

        public FaceDataService(
            IFaceDataRepository faceDataRepository,
            IMapper mapper,
            ILogger<FaceDataService> logger)
        {
            _faceDataRepository = faceDataRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<FaceDataDto>> GetAllAsync()
        {
            try
            {
                var faceDatas = await _faceDataRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<FaceDataDto>>(faceDatas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil semua data wajah");
                throw new ApplicationException("Terjadi kesalahan saat mengambil data wajah");
            }
        }

        public async Task<FaceDataDto> GetByIdAsync(int id)
        {
            try
            {
                var faceData = await _faceDataRepository.GetByIdAsync(id);
                if (faceData == null)
                    throw new ApplicationException("Data wajah tidak ditemukan");

                return _mapper.Map<FaceDataDto>(faceData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil data wajah dengan ID: {Id}", id);
                throw new ApplicationException($"Terjadi kesalahan saat mengambil data wajah dengan ID {id}");
            }
        }

        public async Task<FaceDataDto> GetByUserIdAsync(int userId)
        {
            try
            {
                var faceData = await _faceDataRepository.GetByUserIdAsync(userId);
                if (faceData == null)
                    throw new ApplicationException("Data wajah tidak ditemukan untuk pengguna ini");

                return _mapper.Map<FaceDataDto>(faceData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil data wajah untuk User ID: {UserId}", userId);
                throw new ApplicationException($"Terjadi kesalahan saat mengambil data wajah untuk pengguna {userId}");
            }
        }

        public async Task<FaceDataDto> AddAsync(CreateFaceDataDto createFaceDataDto)
        {
            try
            {
                // Nonaktifkan semua data wajah yang ada untuk user ini
                await _faceDataRepository.DeactivateAllForUserAsync(createFaceDataDto.UserId);

                var faceData = _mapper.Map<FaceData>(createFaceDataDto);
                await _faceDataRepository.AddAsync(faceData);
                return _mapper.Map<FaceDataDto>(faceData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menambahkan data wajah baru");
                throw new ApplicationException("Terjadi kesalahan saat menambahkan data wajah");
            }
        }

        public async Task UpdateAsync(int id, UpdateFaceDataDto updateFaceDataDto)
        {
            try
            {
                var faceData = await _faceDataRepository.GetByIdAsync(id);
                if (faceData == null)
                    throw new ApplicationException("Data wajah tidak ditemukan");

                _mapper.Map(updateFaceDataDto, faceData);
                await _faceDataRepository.UpdateAsync(faceData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memperbarui data wajah dengan ID: {Id}", id);
                throw new ApplicationException($"Terjadi kesalahan saat memperbarui data wajah dengan ID {id}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _faceDataRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menghapus data wajah dengan ID: {Id}", id);
                throw new ApplicationException($"Terjadi kesalahan saat menghapus data wajah dengan ID {id}");
            }
        }

        public async Task DeactivateAllForUserAsync(int userId)
        {
            try
            {
                await _faceDataRepository.DeactivateAllForUserAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menonaktifkan data wajah untuk User ID: {UserId}", userId);
                throw new ApplicationException($"Terjadi kesalahan saat menonaktifkan data wajah untuk pengguna {userId}");
            }
        }
    }
}
