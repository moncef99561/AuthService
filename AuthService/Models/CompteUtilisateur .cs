namespace AuthService.Models
{
    public class CompteUtilisateur
    {
        public int Id { get; set; }
        public int UtilisateurId { get; set; } 
        public string Username { get; set; }
        public string Password { get; set; }
        public string TypeUtilisateur { get; set; }
    }
}
