using ClothStoreApp.Data;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Accounts.Dtos;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Responses;
using ClothStoreApp.Share.Identity;
using ClothStoreApp.Share.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClothStoreApp.Handler.Accounts.Commands
{
    public class LoginCommand : ICommand<LoginCommandResult>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginCommandResult>
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityConfiguration _jwtConfiguration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginCommandHandler(IConfiguration configuration,
            ApplicationDbContext db,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSecurityConfiguration> jwtConfiguration)
        {
            _db = db;
            _configuration = configuration;
            _jwtConfiguration = jwtConfiguration.Value;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<LoginCommandResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            LoginCommandResult result = new();
            
            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
            string passwordHash = hasher.HashPassword(null, request.Password);
            var user = _db.Users.Where(r => r.UserName == request.UserName)
                .FirstOrDefault();

            if(user == null)
            {
                result.IsSuccess = false;
                result.Message = "There're no account matching with provided credentials";
                return result;
            }

            var signInResult = await _signInManager
                .PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: true);

            if(signInResult.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                SetSuccessLoginToken(ref result, user, userRoles);
                result.IsSuccess = true;
                result.Message = "Login successfully";
            }
            else if(signInResult.IsLockedOut)
            {
                result.IsSuccess = false;
                result.Message = "UserIsLockedOut";
            }
            else if(signInResult.IsNotAllowed)
            {
                result.IsSuccess = false;
                result.Message = "UserIsLockedOut";
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "LoginFailedForSomeProblems";
            }

            return result;
        }

        public void SetSuccessLoginToken(ref LoginCommandResult result, ApplicationUser user, ICollection<string> roles)
        {
            //JwtConfigurationDtos jwtInfo = new JwtConfigurationDtos();
            //_configuration.GetSection("JwtConfigurations").Bind(jwtInfo);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecurityKey));
            DateTime expiredTime = DateTime.UtcNow.AddMinutes(10);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = _jwtConfiguration.Audience,
                Issuer = _jwtConfiguration.Issuer,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(IdentityClaimsConst.UserName, user.UserName),
                    new Claim(IdentityClaimsConst.Email, user.Email),
                    new Claim(IdentityClaimsConst.UserId, user.Id.ToString()),
                    //new Claim(IdentityClaimsConst.Roles, string.Join(',', roles)),
                    new Claim(ClaimTypes.Role, string.Join(',', roles))
                }),
                Expires = expiredTime,
                SigningCredentials = new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string jwtToken = tokenHandler.WriteToken(token);
            string stringToken = tokenHandler.WriteToken(token);

            result.Token = stringToken;
            result.ExpiredTime = expiredTime;
        }
    }

    public class LoginCommandResult : BaseResult
    {
        public string Token { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}
