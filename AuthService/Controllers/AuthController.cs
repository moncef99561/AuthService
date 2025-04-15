using AuthService.Data;
using AuthService.Models;
using AuthService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AuthDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("external-register")]
        public async Task<IActionResult> ExternalRegister([FromBody] RegisterFromExternalVM vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Vérifie si l'utilisateur existe déjà
            var exists = _context.CompteUtilisateurs.Any(u => u.Username == vm.Username);
            if (exists)
                return Conflict(new { message = "Ce nom d'utilisateur est déjà enregistré." });

            var compte = new CompteUtilisateur
            {
                Username = vm.Username,
                PasswordHash = vm.Password,
                TypeUtilisateur = vm.TypeUtilisateur,
                UtilisateurId = vm.UtilisateurId
            };

            await _context.AddAsync(compte);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Compte enregistré dans AuthService." });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginVM vm)
        {
            var user = _context.CompteUtilisateurs.FirstOrDefault(u => u.Username == vm.Username);
            if (user == null)
                return Unauthorized(new { message = "Identifiants invalides." });

            var hasher = new PasswordHasher<CompteUtilisateur>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, vm.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { message = "Identifiants invalides." });

            var token = GenerateJwtToken(user);
            return Ok(new
            {
                token,
                user.UtilisateurId,
                user.TypeUtilisateur
            });
        }

        private string GenerateJwtToken(CompteUtilisateur user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UtilisateurId.ToString()),
                new Claim(ClaimTypes.Role, user.TypeUtilisateur),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

