namespace ApplicationGet.Models
{
    public class ReservationFlights
    {
        public IEnumerable<Flight>? ListOfFlights { get; set; }
        public Reservation Reservation { get; set; }
    }
}
