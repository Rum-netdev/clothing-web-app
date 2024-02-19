using ClothStoreApp.Handler.Infrastructures;
using Microsoft.AspNetCore.Mvc;

namespace ClothStoreApp.Web.Controllers
{
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IBroker _broker;

        public CategoriesController(IBroker broker)
        {
            _broker = broker;
        }

        public async Task<IActionResult> CreateCategory([FromForm]CreateCategoryCommand request)
        {
            var result = await _broker.Command(request);
        }
    }
}
