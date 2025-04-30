using AuthService.Data;
using AuthService.Models;
using AuthService.ViewModels;
using AuthService.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly AuthDbContext _context;
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly PasswordHasher<CompteUtilisateur> _passwordHasher;

        public AuthenticationService(AuthDbContext context, IJwtTokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
            _passwordHasher = new PasswordHasher<CompteUtilisateur>();
        }

        public async Task<bool> RegisterAsync(RegisterFromExternalVM vm)
        {
            var exists = await _context.CompteUtilisateurs.AnyAsync(u => u.Username == vm.Username);
            if (exists) return false;

            var compte = CompteUtilisateurMapper.ToEntity(vm);

            await _context.AddAsync(compte);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(string Token, CompteUtilisateur User, string Error)> LoginAsync(LoginVM vm)
        {
            var user = await _context.CompteUtilisateurs
                .FirstOrDefaultAsync(u => u.Username == vm.Username);

            if (user == null)
                return (null, null, "Utilisateur introuvable.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, vm.Password);

            if (result == PasswordVerificationResult.Failed)
                return (null, null, "Mot de passe incorrect.");

            var token = _tokenGenerator.GenerateToken(user);

            return (token, user, null);
        }
    }
}
