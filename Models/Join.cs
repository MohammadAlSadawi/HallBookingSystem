namespace FirstProjectHallBooking.Models
{
    public class Join
    {
        public User user { get; set; }
        public Hall hall { get; set; }
        public UserHall userhall { get; set; }
        public Address address { get; set; }
    }
}
