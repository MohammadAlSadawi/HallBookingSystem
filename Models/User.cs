using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace FirstProjectHallBooking.Models
{
    public partial class User
    {
        public User()
        {
            Testimonials = new HashSet<Testimonial>();
            UserHalls = new HashSet<UserHall>();
            UsersLogins = new HashSet<UsersLogin>();
        }

        public decimal Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? Phone { get; set; }
        public string Address { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public virtual ICollection<Testimonial> Testimonials { get; set; }
        public virtual ICollection<UserHall> UserHalls { get; set; }
        public virtual ICollection<UsersLogin> UsersLogins { get; set; }
    }
}
