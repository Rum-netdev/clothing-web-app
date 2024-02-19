using ClothStoreApp.Data;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Responses;

namespace ClothStoreApp.Handler.Categories.Commands
{
    public class CreateCategoryCommand : ICommand<CreateCategoryCommandResult>
    {
    }

    public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CreateCategoryCommandResult>
    {
        private readonly ApplicationDbContext _db;

        public CreateCategoryCommandHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<CreateCategoryCommandResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class CreateCategoryCommandResult : BaseResult
    {

    }
}
