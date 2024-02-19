using ClothStoreApp.Data;
using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Responses;
using Microsoft.AspNetCore.Identity;

namespace ClothStoreApp.Handler.Accounts.Commands
{
    public class SignUpCommand : ICommand<SignUpCommandResult>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
    }

    public class SignUpCommandHandler : ICommandHandler<SignUpCommand, SignUpCommandResult>
    {
        private readonly ApplicationDbContext _db;
        public SignUpCommandHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<SignUpCommandResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            SignUpCommandResult result = new();

            //ApplicationUser userExisted = _db.Users.Where(t => t.UserName.Equals(request.UserName) || t.Email.Equals(request.Email))
            //    .FirstOrDefault();

            if(_db.Users.Any(t => t.UserName.Equals(request.UserName)))
            {
                result.Message = "UserName has been used, please try another";
                result.IsSuccess = false;
                return result;
            }

            if(_db.Users.Any(t => t.Email.Equals(request.Email)))
            {
                result.Message = "The provided email has been used, please try another";
                result.IsSuccess = false;
                return result;
            }

            PasswordHasher<ApplicationUser> hasher = new();

            ApplicationUser user = new ApplicationUser()
            {
                UserName = request.UserName,
                NormalizedUserName = request.UserName.ToUpper(),
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpper(),
                PhoneNumber = request.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Dob = request.Dob,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            user.PasswordHash = hasher.HashPassword(null, request.Password);

            _db.Users.Add(user);
            int affectedRows = await _db.SaveChangesAsync();
            if(affectedRows > 0)
            {
                await SetBasicRoleToUser(user);
                result.Message = "Create account successfully";
                result.IsSuccess = true;
            }
            else
            {
                result.Message = "Create account failure, please try again";
                result.IsSuccess = false;
            }

            return result;
        }

        private async Task SetBasicRoleToUser(ApplicationUser user)
        {
            ApplicationRole basicRole = _db.Roles.Where(t => t.Name == "Basic").FirstOrDefault();
            _db.UserRoles.Add(new IdentityUserRole<int>() { UserId = user.Id, RoleId = basicRole.Id });
            await _db.SaveChangesAsync();
        }
    }

    public class SignUpCommandResult : BaseResult
    {
    }
}
