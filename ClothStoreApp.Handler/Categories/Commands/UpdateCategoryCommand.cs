using AutoMapper;
using ClothStoreApp.Data;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Categories.Dtos;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Responses;

namespace ClothStoreApp.Handler.Categories.Commands
{
    public class UpdateCategoryCommand : CategoryDto, ICommand<UpdateCategoryCommandResult>
    {
    }

    public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, UpdateCategoryCommandResult>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(ApplicationDbContext db,
            IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<UpdateCategoryCommandResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            UpdateCategoryCommandResult result = new();

            Category existedCat = _db.Categories.Where(t => t.Id == request.Id).FirstOrDefault();
            if(existedCat == null)
            {
                result.IsSuccess = false;
                result.Message = $"There're no categories matching with ID {request.Id}, please try again";
                return result;
            }

            existedCat.Name = request.Name;
            existedCat.Description = request.Description;
            existedCat.ParentId = request.ParentId;

            int affectedRows = await _db.SaveChangesAsync();
            if(affectedRows > 0)
            {
                result.IsSuccess = true;
                result.Message = $"Update category successfully";
            }
            else
            {
                result.IsSuccess = true;
                result.Message = $"Update category failure";
            }

            return result;
        }
    }

    public class UpdateCategoryCommandResult : BaseResult<CategoryDto>
    {
    }
}
