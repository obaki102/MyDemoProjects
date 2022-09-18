using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyDemoProjects.Server.Application.Features.Authentication.Utility;
using MyDemoProjects.Server.Data;
using MyDemoProjects.Server.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyDemoProjects.Server.Application.Features.Authentication.Commands
{
    public record LoginUser(LoginUserRequest User) : IRequest<ServerResponse<string>>;

    public class LoginUserHandler : IRequestHandler<LoginUser, ServerResponse<string>>
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public LoginUserHandler(DataContext applicationDbContext, IConfiguration configuration)
        {
            _dataContext = applicationDbContext;
            _configuration = configuration; 
        }
        public async Task<ServerResponse<string>> Handle(LoginUser request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Email.ToLower().Equals(request.User.Email.ToLower()));
            //Check if user exist
            if (user == null)
            {
                return new ServerResponse<string>
                {
                    Message = "User not found.",
                    Status = false
                };
            }

            //Check if user claims who he is by verifying the current password
            if (!PasswordHash.Verify(request.User.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new ServerResponse<string>
                {
                    Message = "Wrong email or password.",
                    Status = false
                };
            }

            return new ServerResponse<string>
            {
                Data = CreateToken(user),
                Message = "Login Successful",
                Status = true
            };

        }


        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role,user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("MyDemoProjects:SecretKey").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }

}

