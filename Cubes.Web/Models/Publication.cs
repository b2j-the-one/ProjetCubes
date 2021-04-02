using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cubes.Web.Models
{
    public class Publication
    {
        //[Key]
        public int IdPublication { get; set; }

        public string Titre { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}