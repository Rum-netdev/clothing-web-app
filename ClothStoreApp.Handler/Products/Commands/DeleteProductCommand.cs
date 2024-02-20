using AutoMapper;
using ClothStoreApp.Data;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Products.Dtos;
using ClothStoreApp.Handler.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothStoreApp.Handler.Products.Commands
{
    public class DeleteProductCommand : ICommand<DeleteProductCommandResult>
    {
        public int Id { get; set; }
    }

    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, DeleteProductCommandResult>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public DeleteProductCommandHandler(ApplicationDbContext db,
            IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<DeleteProductCommandResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var result = new DeleteProductCommandResult();

            Product exist = _db.Products.Where(t => t.Id == request.Id).FirstOrDefault();
            if(exist == null)
            {
                result.IsSuccess = true;
                result.Message = "Delete product successfully";
                return result;
            }

            ProductDto entityDtoData = _mapper.Map<Product, ProductDto>(exist);

            _db.Products.Remove(exist);
            int affectedRows = await _db.SaveChangesAsync();
            
            result.IsSuccess = affectedRows > 0;
            result.Message = affectedRows > 0 ?
                "Delete product successfully" :
                "Delete product failure";
            result.Data = entityDtoData;

            return result;
        }
    }

    public class DeleteProductCommandResult : BaseResult<ProductDto>
    {

    }
}
