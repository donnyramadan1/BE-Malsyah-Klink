using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Domain.Entities;
using APIKlinik.Domain.Interfaces;
using APIKlinik.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIKlinik.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly APIDbContext _dbContext;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IRepository<User> userRepository,
            IConfiguration configuration,
            IMapper mapper,
            APIDbContext dbContext,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _dbContext.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                            .ThenInclude(r => r.MenuRoles)
                                .ThenInclude(mr => mr.Menu)
                    .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

                if (user == null || !VerifyPassword(loginDto.Password, user.Password))
                {
                    _logger.LogWarning("Login gagal: user tidak ditemukan atau password salah untuk username: {Username}", loginDto.Username);
                    return null;
                }

                // Update last login
                user.LastLogin = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                await _userRepository.UpdateAsync(user);

                // Generate token
                var token = GenerateJwtToken(user);

                // Get distinct roles and menus
                var roles = user.UserRoles.Select(ur => ur.Role).Distinct().ToList();
                var menus = roles.SelectMany(r => r.MenuRoles.Select(mr => mr.Menu)).Distinct().ToList();

                // Map to DTOs
                var userDto = _mapper.Map<UserDto>(user);
                var roleDtos = _mapper.Map<List<RoleDto>>(roles);
                var menuDtos = _mapper.Map<List<MenuDto>>(menus);

                return new AuthResponseDto
                {
                    Token = token,
                    Expiration = DateTime.SpecifyKind(
                        DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                        DateTimeKind.Unspecified),
                    User = userDto,
                    Roles = roleDtos,
                    Menus = menuDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi error saat login user: {Username}", loginDto.Username);
                throw; // dilempar ke controller agar bisa dikembalikan sebagai 500 response
            }
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];
            var jwtExpire = _configuration["Jwt:ExpireMinutes"];

            if (string.IsNullOrWhiteSpace(jwtKey) || string.IsNullOrWhiteSpace(jwtIssuer) ||
                string.IsNullOrWhiteSpace(jwtAudience) || string.IsNullOrWhiteSpace(jwtExpire))
            {
                throw new Exception("Konfigurasi JWT tidak lengkap");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.SpecifyKind(
                DateTime.Now.AddMinutes(Convert.ToDouble(jwtExpire)),
                DateTimeKind.Unspecified);

            var token = new JwtSecurityToken(
                jwtIssuer,
                jwtAudience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // ⚠️ PERINGATAN: Ini hanya untuk contoh.
            // Gunakan BCrypt/Argon2 untuk produksi.
            return password == storedHash;
        }
    }
}
