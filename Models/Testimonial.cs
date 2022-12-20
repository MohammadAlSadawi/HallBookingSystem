using System;
using System.Collections.Generic;

#nullable disable

namespace FirstProjectHallBooking.Models
{
    public partial class Testimonial
    {
        public decimal Id { get; set; }
        public string Feedback { get; set; }
        public decimal? UserId { get; set; }
        public bool? State { get; set; }

        public virtual User User { get; set; }
    }
}
