using ClothStoreApp.Data;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Responses;
using ClothStoreApp.Handler.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostEnv;
        private readonly HttpContext _httpContext;

        public GenerateEmailConfirmationTokenCommandHandler(
            ApplicationDbContext db,
            IEmailService emailService,
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment hostEnv,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _emailService = emailService;
            _userManager = userManager;
            _hostEnv = hostEnv;
            _httpContext = httpContextAccessor.HttpContext;
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

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            // Be carefull when embedded this token to url, cause it maybe contains some characters that make
            // routing can not determine its a part of token or a part of url, so we resolve it by:
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            string templatePath = Path.Combine(_hostEnv.WebRootPath, "assets", "EmailConfirmationContentTemplate.txt");
            string content = File.ReadAllText(templatePath);
            var confirmationLink =
                $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host}/ConfirmUserMail" +
                $"?uid={user.Id}&confirmToken={token}";

            content = string.Format(content, $"{user.FirstName} {user.LastName}", confirmationLink);

            Message smtpMessage = new Message(
                new List<string>() { $"{user.Email}" },
                $"Email confirmation link to user on ClothStoreApp",
                content);
            _emailService.SendEmail(smtpMessage);

            result.Message = "We have sent a mail to your email, it includes email confirmation url to your account, please check it.";
            result.IsSuccess = true;
            return result;
        }
    }

    public class GenerateEmailConfirmationTokenCommandResult : BaseResult
    {
    }
}
