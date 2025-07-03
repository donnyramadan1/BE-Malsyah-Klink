using APIKlinik.Application.DTOs;
using APIKlinik.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIKlinik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> Get()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(ApiResponse<IEnumerable<UserDto>>.Success(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Get(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(ApiResponse<UserDto>.NotFound("User not found"));

            return Ok(ApiResponse<UserDto>.Success(user));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<UserDto>>> Post([FromBody] CreateUserDto createUserDto)
        {
            var user = await _userService.AddUserAsync(createUserDto);
            return CreatedAtAction(nameof(Get), new { id = user.Id },
                ApiResponse<UserDto>.Success("User created successfully", user));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Put(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            await _userService.UpdateUserAsync(id, updateUserDto);
            return Ok(ApiResponse<bool>.Success("User updated successfully", true));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok(ApiResponse<bool>.Success("User deleted successfully", true));
        }
    }
}
