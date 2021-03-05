using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cubes.Web.Models
{
    [NotMapped]
    public class UserView : User
    {
        public HttpPostedFileBase PhotoFile { get; set; }
    }
}