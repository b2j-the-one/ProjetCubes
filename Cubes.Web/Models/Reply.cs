using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cubes.Web.Models
{
    public class Reply
    {
        [Key]
        public int IdReply { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        [Display(Name = "Entrez votre réponse")]
        public string Text { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? DateReponse { get; set; }

        public int IdUser { get; set; }

        [ForeignKey("IdUser")]
        public virtual User User { get; set; }

        public int IdMessage { get; set; }

        [ForeignKey("IdMessage")]
        public virtual Message Message { get; set; }
    }
}