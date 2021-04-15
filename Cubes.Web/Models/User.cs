using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cubes.Web.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        [StringLength(100, ErrorMessage = "Ce champ doit contenir au maximum {1} caractères et au minimum {2}", MinimumLength = 7)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Veuillez saisir une adresse mail valide")]
        [Index("Index_Email", IsUnique = true)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        [StringLength(50, ErrorMessage = "Ce champ doit contenir au maximum {1} caractères et au minimum {2}", MinimumLength = 3)]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        [StringLength(50, ErrorMessage = "Ce champ doit contenir au maximum {1} caractères et au minimum {2}", MinimumLength = 3)]
        public string Prenom { get; set; }

        [Display(Name = "Nom")]
        public string NomComplet => $"{this.Prenom} {this.Nom}";
        //{
        //    get
        //    {
        //        return string.Format("{0} {1}", Prenom, Nom);
        //    }
        //}

        [Display(Name = "Téléphone")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date de naissance")]
        public DateTime DateNaissance { get; set; }

        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date inscription")]
        [ScaffoldColumn(false)]
        public DateTime? DateInscription { get; set; }

        //[Required(ErrorMessage = "Ce champ est obligatoire")]
        [Display(Name = "Etat du compte")]
        public bool IsActivated { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date bannissement")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        [ScaffoldColumn(false)]
        public DateTime? DateBannissement { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date fin bannissement")]
        [ScaffoldColumn(false)]
        public DateTime? DateFinBannissement { get; set; }

        [StringLength(200, ErrorMessage = "Ce champ doit contenir au maximum {1} caractères et au minimum {2}", MinimumLength = 5)]
        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; }

        public string ImageFullPath => string.IsNullOrEmpty(Photo)
            ? null
            : $"http://prolink.somee.com{Photo.Substring(1)}";

        //public string ImageFullPath
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(Photo))
        //        {
        //            return $"http://prolink.somee.com{ImageUrl.Substring(1)}";
        //        }

        //        return null;
        //    }
        //}

        //public virtual ICollection<Message> Messages { get; set; }

        //public virtual ICollection<Reply> Replies { get; set; }
    }
}