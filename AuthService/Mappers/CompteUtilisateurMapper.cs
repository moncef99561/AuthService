using AuthService.Models;
using AuthService.ViewModels;

namespace AuthService.Mappers
{
    public static class CompteUtilisateurMapper
    {
        public static CompteUtilisateur ToEntity(RegisterFromExternalVM vm)
        {
            return new CompteUtilisateur
            {
                UtilisateurId = vm.UtilisateurId,
                Nom = vm.Nom,
                Prenom = vm.Prenom,
                Username = vm.Username,
                Password = vm.Password,
                TypeUtilisateur = vm.TypeUtilisateur,
            };
        }
    }
}
