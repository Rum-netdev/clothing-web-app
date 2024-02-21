using AutoMapper;
using ClothStoreApp.Data;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.OrderDetails.Dtos;
using ClothStoreApp.Handler.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClothStoreApp.Handler.Orders.Command
{
    public class CreateOrderCommand : ICommand<CreateOrderCommandResult>
    {
        public int CustomerId { get; set; }
        public ICollection<OrderDetailDto> Details { get; set; }
    }

    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderCommandResult>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        //private readonly IHttpContextAccessor _httpContext;

        public CreateOrderCommandHandler(ApplicationDbContext db,
            IMapper mapper
            /*IHttpContextAccessor httpContext*/)
        {
            _db = db;
            _mapper = mapper;
            //_httpContext = httpContext;
        }

        public async Task<CreateOrderCommandResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Look up customer based on ApplicationUser
            var customer = _db.Users.Where(t => t.Id == request.CustomerId).FirstOrDefault();
            if(customer == null)
            {
                return new CreateOrderCommandResult
                {
                    Message = $"Does not exist customer has ID {request.CustomerId}, please try again",
                    IsSuccess = false
                };
            }

            Order order = new Order()
            {
                OrderDate = DateTime.Now,
                ReceiverAddress = customer.Address,
                ReceiverName = customer.FirstName + " " + customer.LastName,
                DeliveryVia = 0
            };

            foreach(var detail in request.Details)
            {
                order.OrderDetails.Add(new OrderDetail()
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice,
                    DiscountPercent = 0,
                });
            }

            _db.Orders.Add(order);
            int affectedRows = await _db.SaveChangesAsync();
            CreateOrderCommandResult result = new CreateOrderCommandResult();
            if(affectedRows > 0)
            {
                result.Message = "Create order successfully";
                result.IsSuccess = true;
                result.OrderId = order.Id;
            }
            else
            {
                result.Message = "Create order failure";
                result.IsSuccess = false;
            }

            return result;
        }
    }

    public class CreateOrderCommandResult : BaseResult
    {
        public int OrderId { get; set; }
    }
}
