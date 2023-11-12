namespace ApplicationGet.Models
{
    public class FlightsCities
    {
        public City FromCity { get; set; }
        public City ToCity { get; set; }

        public IEnumerable<City>? ListOfCities { get; set; }
        public IEnumerable<Flight>? ListOfFlights { get; set; }

    }
}
