using System.Security.Claims;
using ToDoWebApplication.Application.Jwt;
using ToDoWebApplication.Application.Repositories.Interfaces;
using ToDoWebApplication.Application.Services.Interfaces;
using ToDoWebApplication.Contracts.DTOs;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtProvider _jwtProvider;

        public AuthService(IUserRepository userRepository, JwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            var user = await _userRepository.GetByLoginAsync(request.Email);
            if (user != null)
            {
                return false; //login already exists
            }

            var newUser = UserModel.Create(Guid.NewGuid(), request.Email, request.Password);
            await _userRepository.AddAsync(newUser);
            return true;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByLoginAsync(request.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid Login");
            }
            else if (user.ValidatePassword(request.Password) == false)
            {
                throw new UnauthorizedAccessException("Invalid Password");
            }

            var accessToken = _jwtProvider.GenerateAccessToken(user);
            var refreshToken = _jwtProvider.GenerateRefreshToken(user);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponse> RefreshAccessTokenAsync(string refreshToken)
        {
            var principal = _jwtProvider.GetPrincipalFromToken(refreshToken) ??
                throw new UnauthorizedAccessException("Invalid refresh token");
            var userId = (principal.FindFirst(ClaimTypes.NameIdentifier)?.Value) ??
                throw new UnauthorizedAccessException("Invalid token claims");
            var user = await _userRepository.GetByIdAsync(Guid.Parse(userId)) ??
                throw new UnauthorizedAccessException("User not found");

            var newAccessToken = _jwtProvider.GenerateAccessToken(user);

            return new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponse> RefreshRefreshTokenAsync(string refreshToken)
        {
            var principal = _jwtProvider.GetPrincipalFromToken(refreshToken) ??
                throw new UnauthorizedAccessException("Invalid refresh token");
            var userId = (principal.FindFirst(ClaimTypes.NameIdentifier)?.Value) ??
                throw new UnauthorizedAccessException("Invalid token claims");
            var user = await _userRepository.GetByIdAsync(Guid.Parse(userId)) ??
                throw new UnauthorizedAccessException("User not found");

            var newAccessToken = _jwtProvider.GenerateAccessToken(user);
            var newRefreshToken = _jwtProvider.GenerateRefreshToken(user);

            return new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
