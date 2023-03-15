using Forecast.Models;

namespace Forecast.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
    }
}
