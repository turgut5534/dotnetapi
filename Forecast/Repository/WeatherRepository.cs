using Forecast.Data;
using Forecast.Interfaces;
using Forecast.Models;
using System.Security.Cryptography;

namespace Forecast.Repository
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly DataContext _context;
        public WeatherRepository(DataContext context ) 
        {
            _context = context;
        }

     
        public ICollection<Weather> GetWeathers()
        {
            return _context.Weathers.OrderBy(p => p.Id).ToList();
        }
    }
}
