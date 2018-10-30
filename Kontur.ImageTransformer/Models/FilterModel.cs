using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer.Models
{
    public class FilterModel
    {
        [Required]
        public int X { get; set; }
        [Required]
        public int Y { get; set; }
        [Required]
        public int W { get; set; }
        [Required]
        public int H { get; set; }

        public object[] FilterParams { get; set; }
    }
}
