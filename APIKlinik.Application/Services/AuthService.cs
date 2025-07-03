using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using APIKlinik.Domain.Entities;
using APIKlinik.Domain.Interfaces;
using APIKlinik.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        public AuthService(
            IRepository<User> userRepository,
            IConfiguration configuration,
            IMapper mapper,
            APIDbContext dbContext)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // Query user dengan include roles dan menus

            var user = await _dbContext.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.MenuRoles)
                            .ThenInclude(mr => mr.Menu)
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
                return null;

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

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.SpecifyKind(
                DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                DateTimeKind.Unspecified);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // Implementasi verifikasi password
            // Contoh sederhana (untuk production gunakan hashing seperti BCrypt)
            return password == storedHash;
        }
    }    
}
