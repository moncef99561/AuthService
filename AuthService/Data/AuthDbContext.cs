using AuthService.Models;
using Microsoft.EntityFrameworkCore;


namespace AuthService.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
        public DbSet<CompteUtilisateur> CompteUtilisateurs { get; set; }
        //public DbSet<CompteUtilisateur> ComptesUtilisateurs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }

}
