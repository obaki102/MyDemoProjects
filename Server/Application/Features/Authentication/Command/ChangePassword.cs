using Microsoft.EntityFrameworkCore;
using MyDemoProjects.Server.Application.Features.Authentication.Utility;
using MyDemoProjects.Server.Data;
using MyDemoProjects.Server.Domain.Entities;
using System.Security.Claims;

namespace MyDemoProjects.Server.Application.Features.Authentication.Command
{
    public record ChangePassword(ChangePasswordRequest User) : IRequest<ServerResponse<bool>>;

    public class ChangePasswordHandler : IRequestHandler<ChangePassword, ServerResponse<bool>>
    {
        private readonly DataContext _dataContext;

        public ChangePasswordHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ServerResponse<bool>> Handle(ChangePassword request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(e => e.Email.ToLower().Equals(request.User.Email.ToLower()));
            //Check if user exist
            if (user == null)
            {
                return new ServerResponse<bool>
                {
                    Data = false,
                    Message = "Password change failed.",
                    Status = false
                };
            }
            //Check if user claims who he is by verifying the current password
            if (!PasswordHash.Verify(request.User.CurrentPassword, user.PasswordHash, user.PasswordSalt))
            {
                return new ServerResponse<bool>
                {
                    Data = false,
                    Message = "Password change failed. Please check if current password is entered correctly.",
                    Status = false
                };
            }

            PasswordHash.Create(request.User.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var result =  await _dataContext.SaveChangesAsync();

            return new ServerResponse<bool>
            {
                Data = result > 0,
                Message = "Password change success.",
                Status = result > 0
            };


        }
    }
}
