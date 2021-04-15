using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cubes.Web.Models
{
    public class Message
    {
        [Key]
        public int IdMessage { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        [Display(Name = "Entrez votre méssage")]
        public string Text { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? DateEnvoie { get; set; }

        public int IdUser { get; set; }

        [ForeignKey("IdUser")]
        public virtual User User { get; set; }

        public ICollection<Reply> Replies { get; set; }
    }
}