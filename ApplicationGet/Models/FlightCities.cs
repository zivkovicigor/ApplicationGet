namespace ApplicationGet.Models
{
    public class FlightCities
    {
        public IEnumerable<City>? ListOfCities { get; set; }
        public Flight Flight { get; set; }
    }
}
