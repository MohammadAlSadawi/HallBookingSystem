using System;
using System.Collections.Generic;

#nullable disable

namespace FirstProjectHallBooking.Models
{
    public partial class UsersLogin
    {
        public decimal Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public decimal? RoleId { get; set; }
        public decimal? UserId { get; set; }

        public virtual Rolee Role { get; set; }
        public virtual User User { get; set; }
    }
}
