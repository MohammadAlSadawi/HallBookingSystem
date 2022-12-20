using System;
using System.Collections.Generic;

#nullable disable

namespace FirstProjectHallBooking.Models
{
    public partial class Category
    {
        public Category()
        {
            UserHalls = new HashSet<UserHall>();
        }

        public decimal Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserHall> UserHalls { get; set; }
    }
}
