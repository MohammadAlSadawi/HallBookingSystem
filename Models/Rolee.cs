using System;
using System.Collections.Generic;

#nullable disable

namespace FirstProjectHallBooking.Models
{
    public partial class Rolee
    {
        public Rolee()
        {
            UsersLogins = new HashSet<UsersLogin>();
        }

        public decimal Id { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<UsersLogin> UsersLogins { get; set; }
    }
}
