using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cubes.Web.Models
{
    [NotMapped]
    public class UserIndexView : User
    {
        [Display(Name = "Est Super Admin ?")]
        public bool IsSuperAdmin { get; set; }

        [Display(Name = "Est Administrateur ?")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Est Modérateur ?")]
        public bool IsModerator { get; set; }
    }
}