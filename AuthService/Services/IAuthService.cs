using AuthService.ViewModels;

namespace AuthService.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterFromExternalVM vm);
        Task<(string? Token, string? Error)> LoginAsync(LoginVM vm);
    }
}
