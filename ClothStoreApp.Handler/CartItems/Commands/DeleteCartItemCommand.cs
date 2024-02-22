using ClothStoreApp.Data;
using ClothStoreApp.Handler.CartItems.Dtos;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Responses;
using ClothStoreApp.Share.Identity;
using Microsoft.AspNetCore.Http;

namespace ClothStoreApp.Handler.CartItems.Commands
{
    public class DeleteCartItemCommand : ICommand<DeleteCartItemCommandResult>
    {
        public int ItemId { get; set; }
    }

    public class DeleteCartItemCommandHandler : ICommandHandler<DeleteCartItemCommand, DeleteCartItemCommandResult>
    {
        private readonly ApplicationDbContext _db;
        private readonly HttpContext _httpContext;

        public DeleteCartItemCommandHandler(ApplicationDbContext db,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<DeleteCartItemCommandResult> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
        {
            int userId = 0;
            int.TryParse(_httpContext
                .User.Claims.Where(t => t.Type == IdentityClaimsConst.UserId)
                .Select(t => t.Value)
                .FirstOrDefault(), out userId);

            if (!_db.Users.Any(t => t.Id == userId))
            {
                return new DeleteCartItemCommandResult()
                {
                    Message = $"There're no users has ID {userId}",
                    IsSuccess = false
                };
            }

            var item = _db.CartItems.Where(t => t.Id == request.ItemId && t.UserId == userId)
                .FirstOrDefault();

            _db.CartItems.Remove(item);
            int affectedRows = await _db.SaveChangesAsync();


            return new DeleteCartItemCommandResult()
            {
                Message = "Delete item successfully",
                IsSuccess = true
            };
        }
    }

    public class DeleteCartItemCommandResult : BaseResult<CartItemDto>
    {
    }
}
