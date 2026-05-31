using ToDoWebApplication.Contracts.DTOs;
using ToDoWebApplication.Domain.Models;

namespace ToDoWebApplication.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshAccessTokenAsync(string refreshToken);
        Task<AuthResponse> RefreshRefreshTokenAsync(string refreshToken);
        Task<UserModel?> AuthenticateAsync(string email, string password);
    }
}
