using AutoMapper;
using ClothStoreApp.Data;
using ClothStoreApp.Handler.Categories.Dtos;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Responses;

namespace ClothStoreApp.Handler.Categories.Commands
{
    public class DeleteCategoryCommand : ICommand<DeleteCategoryCommandResult>
    {
        public int CategoryId { get; set; }
    }

    public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, DeleteCategoryCommandResult>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public DeleteCategoryCommandHandler(ApplicationDbContext db,
            IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<DeleteCategoryCommandResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            DeleteCategoryCommandResult result = new();
            var category = _db.Categories.Where(t => t.Id == request.CategoryId).FirstOrDefault();
            
            if(category == null)
            {
                result.IsSuccess = true;
                result.Message = "Delete category successfully";
                return result;
            }

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            result.Data = _mapper.Map<CategoryDto>(category);
            result.IsSuccess = true;
            result.Message = "Delete category successfully";
            return result;
        }
    }

    public class DeleteCategoryCommandResult : BaseResult<CategoryDto>
    {
    }
}
