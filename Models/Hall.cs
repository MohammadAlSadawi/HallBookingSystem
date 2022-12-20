using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace FirstProjectHallBooking.Models
{
    public partial class Hall
    {
        public Hall()
        {
            UserHalls = new HashSet<UserHall>();
        }

        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? AddressId { get; set; }
        public string Imagepath { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<UserHall> UserHalls { get; set; }
    }
}
