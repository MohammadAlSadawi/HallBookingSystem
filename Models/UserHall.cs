using System;
using System.Collections.Generic;

#nullable disable

namespace FirstProjectHallBooking.Models
{
    public partial class UserHall
    {
        public decimal Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsApproved { get; set; }
        public decimal? HallId { get; set; }
        public decimal? UserId { get; set; }
        public decimal? CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Hall Hall { get; set; }
        public virtual User User { get; set; }
    }
}
