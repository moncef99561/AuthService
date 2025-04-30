using System.ComponentModel.DataAnnotations;

namespace AuthService.Models
{
    public class CompteUtilisateur
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UtilisateurId { get; set; }

        [Required]
        [StringLength(50)]
        public string Nom { get; set; }

        [Required]
        [StringLength(50)]
        public string Prenom { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(20)]
        public string TypeUtilisateur { get; set; } 
    }
}
