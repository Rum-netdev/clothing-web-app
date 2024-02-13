using AutoMapper;
using ClothStoreApp.Data;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Products.Dtos;
using ClothStoreApp.Handler.Responses;

namespace ClothStoreApp.Handler.Products.Commands
{
    public class CreateProductCommand : ProductDto, ICommand<CreateProductCommandResult>
    {
    }

    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductCommandResult>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(ApplicationDbContext db,
            IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<CreateProductCommandResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            CreateProductCommandResult result = new();
            var product = _mapper.Map<ProductDto, Product>(request);
            _db.Products.Add(product);
            int affectedRows = await _db.SaveChangesAsync();

            result.IsSuccess = affectedRows > 0;
            result.Message = affectedRows > 0 ?
                "Created new product successfully" :
                "Created new product failure";

            return result;
        }
    }

    public class CreateProductCommandResult : BaseResult
    {
    }
}
