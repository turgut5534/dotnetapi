using Forecast.Interfaces;
using Forecast.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using MvcRoute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Forecast.Controllers
{
    [MvcRoute("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers( )
        {
            var users = _userRepository.GetUsers();

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }
    }
}
