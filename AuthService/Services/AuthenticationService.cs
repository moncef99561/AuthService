using AuthService.Data;
using AuthService.Models;
using AuthService.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly AuthDbContext _context;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public AuthenticationService(AuthDbContext context, IJwtTokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<bool> RegisterAsync(RegisterFromExternalVM vm)
        {
            var exists = _context.CompteUtilisateurs.Any(u => u.Username == vm.Username);
            if (exists) return false;

            var compte = new CompteUtilisateur
            {
                Username = vm.Username,
                Password = vm.Password,
                TypeUtilisateur = vm.TypeUtilisateur,
                UtilisateurId = vm.UtilisateurId
            };

            await _context.AddAsync(compte);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(string? Token, string? Error)> LoginAsync(LoginVM vm)
        {
            var user = _context.CompteUtilisateurs.FirstOrDefault(u => u.Username == vm.Username);
            if (user == null)
                return (null, "Nom d'utilisateur ou mot de passe incorrect.");

            var hasher = new PasswordHasher<CompteUtilisateur>();
            var result = hasher.VerifyHashedPassword(user, user.Password, vm.Password);

            if (result == PasswordVerificationResult.Failed)
                return (null, "Nom d'utilisateur ou mot de passe incorrect.");

            var token = _tokenGenerator.GenerateToken(user);
            return (token, null);
        }
    }

}
