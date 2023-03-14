using Forecast.Data;
using Forecast.Models;
using System.Diagnostics.Metrics;

namespace Forecast
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.Users.Any())
            {
                var users = new List<User>()
        {
            new User
                {
                    UserId = 1,
                    Username = "John Doe",
                    Email = "johndoe@example.com",
                    Password = "John123",
                    CreatedAt = DateTime.UtcNow,
                },
               new User
               {
                   UserId = 2,
                   Username = "Turgut Salgın",
                   Email = "turgut@example.com",
                   Password = "Turgut123",
                   CreatedAt = DateTime.UtcNow,
               }
        };
                dataContext.Users.AddRange(users);
                dataContext.SaveChanges();
            }
        }
    }
}