using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cubes.Web.Models
{
    public class Commentaire
    {
        [Key]
        public int IdCommentaire { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        //[Display(Name = "Entrer un commentaire...")]
        public string Text { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? DateCommentaire { get; set; }

        public int IdUser { get; set; }

        public int IdRessource { get; set; }

        [ForeignKey("IdUser")]
        public virtual User User { get; set; }

        [ForeignKey("IdRessource")]
        public virtual Ressource Ressource { get; set; }
    }
}