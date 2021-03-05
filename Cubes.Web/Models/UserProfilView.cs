using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Cubes.Web.Models
{
    [NotMapped]
    public class UserProfilView : User
    {
        [Display(Name = "Nouvelle photo")]
        public HttpPostedFileBase NewPhoto { get; set; }
    }
}