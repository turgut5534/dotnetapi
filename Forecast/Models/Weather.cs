using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Forecast.Models
{
    public class Weather
    {
        public int Id { get; set; }
        public long Dt { get; set; }
        public float Temperature { get; set; }
        public float TempMin { get; set; }
        public float TempMax { get; set; }
        public int Pressure { get; set; }
        public int SeaLevel { get; set; }
        public int GrndLevel { get; set; }
        public int Humidity { get; set; }
        public float WindSpeed { get; set; }
        public float WindDeg { get; set; }
        public float WindGust { get; set; }
        public int? Clouds { get; set; }
        public int? Visibility { get; set; }
        public int WeatherId { get; set; }
        public string? WeatherMain { get; set; }
        public string? WeatherDescription { get; set; }
        public string? WeatherIcon { get; set; }
        public float? Rain1h { get; set; }
        public float? Rain3h { get; set; }
        public float? Snow1h { get; set; }
        public float? Snow3h { get; set; }
        public string icon { get; set; }
        public string dt_txt { get; set; }
        public int WeatherLocationId { get; set; }
        public WeatherLocation WeatherLocation { get; set; }


    }
}
