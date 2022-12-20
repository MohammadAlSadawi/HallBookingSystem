namespace FirstProjectHallBooking.Models
{
    public class JoinTables
    {
        public User user { get; set; }
        public Hall hall { get; set; }
        public UserHall userhall { get; set; }
        public Category category { get; set; }
    }
}
