using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIKlinik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var response = await _authService.LoginAsync(loginDto);
                if (response == null)
                    return Unauthorized(ApiResponse<AuthResponseDto>.Unauthorized("Email atau password salah"));

                return Ok(ApiResponse<AuthResponseDto>.Success("Login berhasil", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AuthResponseDto>.InternalError("Terjadi kesalahan saat login: " + ex.Message));
            }
        }
    }
}
