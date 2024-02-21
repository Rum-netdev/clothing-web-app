using AutoMapper;
using ClothStoreApp.Data;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Products.Dtos;
using ClothStoreApp.Handler.Responses;

namespace ClothStoreApp.Handler.Products.Queries
{
    public class GetProductByIdQuery : IQuery<GetProductByIdQueryResult>
    {
        public int ProductId { get; set; }
    }

    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdQueryResult>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(ApplicationDbContext db,
            IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<GetProductByIdQueryResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            GetProductByIdQueryResult result = new();

            Product exist = _db.Products.Where(t => t.Id == request.ProductId).FirstOrDefault();
            if(exist == null)
            {
                result.Message = $"There're no products matching with ID {request.ProductId}";
                result.IsSuccess = false;
                return result;
            }

            result.Data = _mapper.Map<Product, ProductDto>(exist);
            result.IsSuccess = true;
            result.Message = "Retrieve 1 product from server successfully";

            return result;
        }
    }

    public class GetProductByIdQueryResult : BaseResult<ProductDto>
    {
    }
}
