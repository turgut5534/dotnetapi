using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Forecast.Models
{
    public class Weather
    {
        [Key]
        public int ForecastId { get; set; }

        [Required]
        public decimal Temperature { get; set; }

        [Required]
        public decimal Humidity { get; set; }

        [Required]
        public decimal WindSpeed { get; set; }

        [Required]
        public string WindDirection { get; set; }

        [Required]
        public string WeatherCondition { get; set; }

        [Required]
        public DateTime ForecastDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [ForeignKey("Location")]
        public int LocationId { get; set; }

        public Location Location { get; set; }
    }
}
