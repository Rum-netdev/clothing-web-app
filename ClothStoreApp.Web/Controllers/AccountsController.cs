using ClothStoreApp.Handler.Accounts.Commands;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Share.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

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

        [HttpPost]
        [Route("/preemailconfirm")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GenerateEmailConfirmationToken(
            GenerateEmailConfirmationTokenCommand command)
        {
            int userId = 0;
            int.TryParse(User.Claims.Where(t => t.Type == IdentityClaimsConst.UserId)
                .Select(t => t.Value).FirstOrDefault(),
                out userId);

            command.UserId = userId != 0 ? userId : 0;
            var result = await _broker.Command(command);
            return Ok(result);
        }


        [HttpGet]
        [Route("/confirmusermail")]
        public async Task<IActionResult> ConfirmUserEmail(
            [FromQuery]ConfirmUserEmailCommand command)
        {
            var result = await _broker.Command(command);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
