using ClothStoreApp.Data;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothStoreApp.Handler.Accounts.Commands
{
    public class ConfirmUserEmailCommand : ICommand<ConfirmUserEmailCommandResult>
    {
        public int Uid { get; set; }
        public string ConfirmToken { get; set; }
    }

    public class ConfirmUserEmailCommandHander : ICommandHandler<ConfirmUserEmailCommand, ConfirmUserEmailCommandResult>
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmUserEmailCommandHander(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<ConfirmUserEmailCommandResult> Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken)
        {
            var user = _db.Users.Where(t => t.Id == request.Uid).FirstOrDefault();
            if(user == null)
            {
                return new ConfirmUserEmailCommandResult()
                {
                    Message = "Invalid user ID",
                    IsSuccess = false
                };
            }

            byte[] codeDecodedBytes = WebEncoders.Base64UrlDecode(request.ConfirmToken);
            string code = Encoding.UTF8.GetString(codeDecodedBytes);

            var isValid = await _userManager.ConfirmEmailAsync(user, code);
            if(isValid.Succeeded)
            {
                return new ConfirmUserEmailCommandResult()
                {
                    Message = "Confirm email successfully",
                    IsSuccess = true
                };
            }

            return new ConfirmUserEmailCommandResult()
            {
                Message = "Invalid Token or something went wrong when confirm email, please try again or contact the provider",
                IsSuccess = false
            };
        }
    }

    public class ConfirmUserEmailCommandResult : BaseResult
    {

    }
}
