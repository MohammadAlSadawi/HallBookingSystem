using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace FirstProjectHallBooking.Models
{
    public partial class Home
    {
        public decimal Id { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public IFormFile ImageFile1 { get; set; }
        [NotMapped]
        public IFormFile ImageFile2 { get; set; }
        public string Description { get; set; }
    }
}
