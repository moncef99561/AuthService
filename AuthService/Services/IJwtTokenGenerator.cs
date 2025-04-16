using AuthService.Models;

namespace AuthService.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(CompteUtilisateur user);
    }
}
