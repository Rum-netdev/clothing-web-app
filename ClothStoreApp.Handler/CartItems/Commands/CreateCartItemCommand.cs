using ClothStoreApp.Data;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Responses;
using ClothStoreApp.Share.Identity;
using Microsoft.AspNetCore.Http;

namespace ClothStoreApp.Handler.CartItems.Commands
{
    public class CreateCartItemCommand : ICommand<CreateCartItemCommandResult>
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
    }

    public class CreateCartItemCommandHandler : ICommandHandler<CreateCartItemCommand, CreateCartItemCommandResult>
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContext;

        public CreateCartItemCommandHandler(ApplicationDbContext db,
            IHttpContextAccessor httpContext)
        {
            _db = db;
            _httpContext = httpContext;
        }

        public async Task<CreateCartItemCommandResult> Handle(CreateCartItemCommand request, CancellationToken cancellationToken)
        {
            int userId = 0;
            int.TryParse(_httpContext.HttpContext
                .User.Claims.Where(t => t.Type == IdentityClaimsConst.UserId)
                .Select(t => t.Value)
                .FirstOrDefault(), out userId);

            if(!_db.Users.Any(t => t.Id == userId))
            {
                return new CreateCartItemCommandResult()
                {
                    Message = $"There're no any user has ID matching with {userId}",
                    IsSuccess = false
                };
            }

            // We can validating ProductId here

            var item = _db.CartItems.Where(t => t.ProductId == request.ProductId && t.UserId == userId).FirstOrDefault();
            if(item == null)
            {
                // Generate CartItem object
                item = new CartItem()
                {
                    Quantity = request.Quantity,
                    UnitPrice = request.UnitPrice,
                    UserId = userId,
                    ProductId = request.ProductId,
                };
                _db.CartItems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + request.Quantity;
                item.UnitPrice = request.UnitPrice;
            }

            int affectedRows = await _db.SaveChangesAsync();
            CreateCartItemCommandResult result = new();

            if(affectedRows > 0)
            {
                result.Message = "Add item to cart successfully";
                result.IsSuccess = true;
                result.ItemId = item.Id;
            }
            else
            {
                result.Message = "Failure when try to add item";
                result.IsSuccess = false;
            }

            return result;
        }
    }

    public class CreateCartItemCommandResult : BaseResult
    {
        public int ItemId { get; set; }
    }
}
