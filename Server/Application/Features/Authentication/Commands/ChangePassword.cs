using Microsoft.EntityFrameworkCore;
using MyDemoProjects.Server.Application.Features.Authentication.Utility;
using MyDemoProjects.Server.Data;
using MyDemoProjects.Server.Domain.Entities;
using System.Security.Claims;

namespace MyDemoProjects.Server.Application.Features.Authentication.Commands
{
    public record ChangePassword(ChangePasswordRequest User1) : IRequest<ServerResponse<bool>>;

    public class ChangePasswordHandler : IRequestHandler<ChangePassword, ServerResponse<bool>>
    {
        private readonly Data.ApplicationDbContext _dataContext;

        public ChangePasswordHandler(Data.ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ServerResponse<bool>> Handle(ChangePassword request, CancellationToken cancellationToken)
        {
            var User1 = await _dataContext.Users.FirstOrDefaultAsync(e => e.Email.ToLower().Equals(request.User1.Email.ToLower()));
            //Check if User1 exist
            if (User1 == null)
            {
                return new ServerResponse<bool>
                {
                    Data = false,
                    Message = "Password change failed.",
                    Status = false
                };
            }
            ////Check if User1 claims who he is by verifying the current password
            //if (!PasswordHash.Verify(request.User1.CurrentPassword, User1.PasswordHash, User1.PasswordSalt))
            //{
            //    return new ServerResponse<bool>
            //    {
            //        Data = false,
            //        Message = "Password change failed. Please check if current password is entered correctly.",
            //        Status = false
            //    };
            //}

            //PasswordHash.Create(request.User1.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            //User1.PasswordHash = passwordHash;
            //User1.PasswordSalt = passwordSalt;

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
