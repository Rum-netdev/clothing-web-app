using AutoMapper;
using ClothStoreApp.Data;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Categories.Dtos;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Responses;

namespace ClothStoreApp.Handler.Categories.Commands
{
    public class CreateCategoryCommand : CategoryDto, ICommand<CreateCategoryCommandResult>
    {
    }

    public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CreateCategoryCommandResult>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(ApplicationDbContext db,
            IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<CreateCategoryCommandResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            CreateCategoryCommandResult result = new CreateCategoryCommandResult();

            if (_db.Categories.Any(t => t.Name == request.Name))
            {
                result.Message = "There's exist a category has the same name, try another";
                result.IsSuccess = false;
                return result;
            }

            Category category = _mapper.Map<Category>(request);
            _db.Categories.Add(category);
            int affectedRows = await _db.SaveChangesAsync();

            if(affectedRows > 0)
            {
                result.Message = "Create category successfully";
                result.IsSuccess = true;
                result.Data = _mapper.Map<CategoryDto>(category);
            }
            else
            {
                result.Message = "Create category failure";
                result.IsSuccess = false;
            }

            return result;
        }
    }

    public class CreateCategoryCommandResult : BaseResult<CategoryDto>
    {
    }
}
