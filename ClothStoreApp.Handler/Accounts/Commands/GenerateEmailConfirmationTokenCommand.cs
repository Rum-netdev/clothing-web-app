using ClothStoreApp.Data;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Responses;
using ClothStoreApp.Handler.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothStoreApp.Handler.Accounts.Commands
{
    public class GenerateEmailConfirmationTokenCommand : ICommand<GenerateEmailConfirmationTokenCommandResult>
    {
        public int UserId { get; set; }
    }

    public class GenerateEmailConfirmationTokenCommandHandler : ICommandHandler<GenerateEmailConfirmationTokenCommand, GenerateEmailConfirmationTokenCommandResult>
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailService _emailService;

        public GenerateEmailConfirmationTokenCommandHandler(
            ApplicationDbContext db,
            IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        public async Task<GenerateEmailConfirmationTokenCommandResult> Handle(
            GenerateEmailConfirmationTokenCommand request, CancellationToken cancellationToken)
        {
            GenerateEmailConfirmationTokenCommandResult result = new();
            // Retrieve current email of user
            var user = _db.Users.Where(t => t.Id == request.UserId).FirstOrDefault();
            if(string.IsNullOrEmpty(user.Email))
            {
                result.Message = "User's email is empty, please add at least one email to perform this action";
                result.IsSuccess = false;
                return result;
            }

            return result;
        }
    }

    public class GenerateEmailConfirmationTokenCommandResult : BaseResult
    {

    }
}
