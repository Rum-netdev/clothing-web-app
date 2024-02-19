using ClothStoreApp.Handler.Accounts.Commands;
using ClothStoreApp.Handler.Infrastructures;
using Microsoft.AspNetCore.Mvc;

namespace ClothStoreApp.Web.Controllers
{
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IBroker _broker;

        public AccountsController(IBroker broker)
        {
            _broker = broker;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromForm] LoginCommand request)
        {
            var result = await _broker.Command(request);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("/signup")]
        public async Task<IActionResult> SignUp([FromForm] SignUpCommand request)
        {
            var result = await _broker.Command(request);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
