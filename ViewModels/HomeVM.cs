using FirstProjectHallBooking.Models;
using System.Collections.Generic;

namespace FirstProjectHallBooking.ViewModels
{
    public class HomeVM
    {
        public List<Hall> halls { get; set; }
        public UserHall UserHall { get; set; }
        public Home home { get; set; }
        public Aboutu Aboutu { get; set; }
        public List<Category> Categories { get; set; }
        public List<Visa> visas { get; set; }
        public Testimonial tests { get; set; }
        public List<Testimonial> testimonials { get; set; }
    }
}
