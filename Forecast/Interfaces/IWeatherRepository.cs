using Forecast.Models;

namespace Forecast.Interfaces
{
    public interface IWeatherRepository
    {
        ICollection<Weather> GetWeathers();

    }
}
