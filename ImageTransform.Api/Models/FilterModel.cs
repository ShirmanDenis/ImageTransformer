using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ImageTransform.Api.Models
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
