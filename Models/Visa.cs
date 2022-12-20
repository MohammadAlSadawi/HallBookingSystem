using System;
using System.Collections.Generic;

#nullable disable

namespace FirstProjectHallBooking.Models
{
    public partial class Visa
    {
        public decimal Id { get; set; }
        public decimal? CardNumber { get; set; }
        public decimal? Balnce { get; set; }
        public decimal? Cvv { get; set; }
    }
}
