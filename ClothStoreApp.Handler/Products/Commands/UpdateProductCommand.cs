using AutoMapper;
using ClothStoreApp.Data;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Products.Dtos;
using ClothStoreApp.Handler.Responses;

namespace ClothStoreApp.Handler.Products.Commands
{
    public class UpdateProductCommand : ProductDto, ICommand<UpdateProductCommandResult>
    {
        public int Id { get; set; }
    }


    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductCommandResult>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(ApplicationDbContext db,
            IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<UpdateProductCommandResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var result = new UpdateProductCommandResult();

            Product exist = _db.Products.Where(t => t.Id == request.Id).FirstOrDefault();
            if(exist == null)
            {
                result.Message = $"The product with ID is {request.Id} is not existing";
                result.IsSuccess = false;
                return result;
            }

            exist.ProductName = request.ProductName;
            exist.ProductDescription = request.ProductDescription;
            exist.UnitPrice = request.UnitPrice;
            exist.QuantityPerUnit = request.QuantityPerUnit;
            exist.UnitsInStock = request.UnitsInStock;

            int affectedRows = await _db.SaveChangesAsync();

            result.IsSuccess = affectedRows > 0;
            result.Message = affectedRows > 0 ?
                $"Update the product has ID is {request.Id} successfully" :
                $"Update the product has ID is {request.Id} failure";

            result.Data = affectedRows > 0 ?
                _mapper.Map<Product, ProductDto>(exist) :
                null;

            return result;
        }
    }

    public class UpdateProductCommandResult : BaseResult<ProductDto>
    {
    }
}
