using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Cubes.Web.Models
{
    [NotMapped]
    public class RessourceFileView : Ressource
    {
        [Display(Name = "Nouveau fichier")]
        public HttpPostedFileBase NewFile { get; set; }
    }
}