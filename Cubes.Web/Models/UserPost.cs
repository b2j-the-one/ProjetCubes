using System.ComponentModel.DataAnnotations.Schema;

namespace Cubes.Web.Models
{
    [NotMapped]
    public class UserPost : User
    {
        public string Password { get; set; }
    }
}