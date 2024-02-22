using ClothStoreApp.Handler.CartItems.Commands;
using ClothStoreApp.Handler.Infrastructures;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClothStoreApp.Web.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CartItemsController : ControllerBase
    {
        private readonly IBroker _broker;

        public CartItemsController(IBroker broker)
        {
            _broker = broker;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCartItem([FromBody]CreateCartItemCommand request)
        {
            var result = await _broker.Command(request);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCartItem([FromQuery]DeleteCartItemCommand request)
        {
            var result = await _broker.Command(request);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
