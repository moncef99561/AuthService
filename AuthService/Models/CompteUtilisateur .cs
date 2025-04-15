namespace AuthService.Models
{
    public class CompteUtilisateur
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string TypeUtilisateur { get; set; }
        public int UtilisateurId { get; set; } 
    }
}
