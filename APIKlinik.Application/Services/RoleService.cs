using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Domain.Entities;
using APIKlinik.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace APIKlinik.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _logger;

        public RoleService(IRoleRepository roleRepository, IMapper mapper, ILogger<RoleService> logger)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _roleRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<RoleDto>>(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil data semua role.");
                throw new ApplicationException("Terjadi kesalahan saat mengambil data role.");
            }
        }

        public async Task<RoleDto> GetRoleByIdAsync(int id)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                    throw new ApplicationException("Role tidak ditemukan.");

                return _mapper.Map<RoleDto>(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mengambil role dengan ID: {RoleId}", id);
                throw new ApplicationException("Terjadi kesalahan saat mengambil detail role.");
            }
        }

        public async Task<PagedResult<RoleDto>> GetPagedRolesAsync(int page, int pageSize, string? search = null)
        {
            try
            {
                Expression<Func<Role, bool>>? filter = null;

                if (!string.IsNullOrEmpty(search))
                {
                    filter = r => r.Name.ToLower().Contains(search.ToLower()) ||
                                 r.Description.ToLower().Contains(search.ToLower());
                }

                var result = await _roleRepository.GetPagedAsync(page, pageSize, filter);
                return new PagedResult<RoleDto>
                {
                    Items = _mapper.Map<IEnumerable<RoleDto>>(result.Items),
                    TotalItems = result.TotalItems,
                    Page = result.Page,
                    PageSize = result.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memuat data role dengan pagination.");
                throw new ApplicationException("Terjadi kesalahan saat memuat data role.");
            }
        }

        public async Task<RoleDto> AddRoleAsync(CreateRoleDto createRoleDto)
        {
            try
            {
                var role = _mapper.Map<Role>(createRoleDto);
                await _roleRepository.AddAsync(role);
                return _mapper.Map<RoleDto>(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menambahkan role baru.");
                throw new ApplicationException("Terjadi kesalahan saat menambahkan role.");
            }
        }

        public async Task UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                    throw new ApplicationException("Role tidak ditemukan untuk diperbarui.");

                _mapper.Map(updateRoleDto, role);
                await _roleRepository.UpdateAsync(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal memperbarui role dengan ID: {RoleId}", id);
                throw new ApplicationException("Terjadi kesalahan saat memperbarui role.");
            }
        }

        public async Task DeleteRoleAsync(int id)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                    throw new ApplicationException("Role tidak ditemukan untuk dihapus.");

                await _roleRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal menghapus role dengan ID: {RoleId}", id);
                throw new ApplicationException("Terjadi kesalahan saat menghapus role.");
            }
        }
    }
}
