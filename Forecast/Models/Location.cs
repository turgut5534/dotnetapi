using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Forecast.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required]
        public string LocationName { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<Weather> Forecasts { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
