namespace Forecast.Models
{
    public class WeatherLocation
    {
        public WeatherLocation()
        {
            Weathers = new HashSet<Weather>();
        }
        public int Id { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public ICollection<Weather> Weathers { get; set; }

    }
}
