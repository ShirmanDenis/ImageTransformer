using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Kontur.ImageTransformer.Models
{
    public class FilterModel
    {
        [Required]
        [FromQuery(Name = "x")]
        public int X { get; set; }
        [Required]
        [FromQuery(Name = "y")]
        public int Y { get; set; }
        [Required]
        [FromQuery(Name = "w")]
        public int W { get; set; }
        [Required]
        [FromQuery(Name = "h")]
        public int H { get; set; }

        public object[] FilterParams { get; set; }
    }
}
