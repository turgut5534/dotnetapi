using Forecast.Data;
using Forecast.Interfaces;
using Forecast.Models;

namespace Forecast.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context ) 
        {
            _context = context;
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.OrderBy(p => p.UserId).ToList();
        }
    }
}
