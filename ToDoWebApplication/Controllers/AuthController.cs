using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;
namespace ToDoWebApplication.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequest request)
        {
            bool result = await _authService.RegisterAsync(request);

            if (!result)
                return BadRequest("User already exists");
            return Ok("User registered successfully");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh-access")]
        public async Task<IActionResult> Refresh([FromBody] RefreshJwtRequest request)
        {
            try
            {
                var response = await _authService.RefreshAccessTokenAsync(request.RefreshToken);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("refresh-refresh")]
        public async Task<IActionResult> RefreshRefresh([FromBody] RefreshJwtRequest request)
        {
            try
            {
                var response = await _authService.RefreshRefreshTokenAsync(request.RefreshToken);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
