using AuthService.Models;
using AuthService.ViewModels;

namespace AuthService.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterFromExternalVM vm);
        Task<(string Token, CompteUtilisateur User, string? Error)> LoginAsync(LoginVM vm);

    }
}
