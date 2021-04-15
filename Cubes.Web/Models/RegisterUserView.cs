using System.ComponentModel.DataAnnotations;

namespace Cubes.Web.Models
{
    public class RegisterUserView : UserView
    {
        [Required(ErrorMessage = "Ce champ est obligatoire")]
        [StringLength(20, ErrorMessage = "Ce champ doit contenir au maximum {1} caractères et au minimum {2}", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        [StringLength(20, ErrorMessage = "Ce champ doit contenir au maximum {1} caractères et au minimum {2}", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmer")]
        [Compare("Password", ErrorMessage = "Les deux mots de passe ne correspondent pas")]
        public string ConfirmPassword { get; set; }
    }
}