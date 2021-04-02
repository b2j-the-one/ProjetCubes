using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cubes.Web.Models
{
    public class Ressource
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        [StringLength(50, ErrorMessage = "Ce champ doit contenir au maximum {1} caractères et au minimum {2}", MinimumLength = 3)]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        [Display(Name = "Catégorie ressource")]
        public int IdCategorie { get; set; }

        //[Required(ErrorMessage = "Ce champ est obligatoire")]
        [StringLength(50, ErrorMessage = "Ce champ doit contenir au maximum {1} caractères et au minimum {2}", MinimumLength = 3)]
        [Display(Name = "Titre")]
        public string Titre { get; set; }

        [Display(Name = "Est privée")]
        public bool IsPrivate { get; set; }

        [Display(Name = "A été modérée")]
        public bool IsValid { get; set; }

        [Display(Name = "A été publiée")]
        public bool IsPublish { get; set; }

        [Display(Name = "Est favorite")]
        public bool IsFavorite { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date de création")]
        [NotMapped]
        public DateTime? DateCreation { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date de publication")]
        [NotMapped]
        public DateTime? DatePublication { get; set; }

        [StringLength(200, ErrorMessage = "Ce champ doit contenir au maximum {1} caractères et au minimum {2}", MinimumLength = 5)]
        [Display(Name = "Joindre fichier")]
        public string Fichier { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public string fichierFullPath => string.IsNullOrEmpty(Fichier)
            ? null
            : $"http://prolink.somee.com{Fichier.Substring(1)}";

        public virtual Categorie Categorie { get; set; }
    }
}