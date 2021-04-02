using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cubes.Web.Models
{
    public class Categorie
    {
        [Key]
        public int IdCategorie { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        [StringLength(50, ErrorMessage = "Ce champ doit contenir au maximum {1} caractères et au minimum {2}", MinimumLength = 3)]
        [Display(Name = "Catégorie")]
        public string Libelle { get; set; }

        public virtual ICollection<Ressource> Ressources { get; set; }
    }
}