using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyDemoProjects.Server.Application.Features.Authentication.Utility;
using MyDemoProjects.Server.Data;
using MyDemoProjects.Server.Domain.Entities;
using System.Security.Cryptography;

namespace MyDemoProjects.Server.Application.Features.Authentication.Command
{
    public record CreateAccount(UserDto User) : IRequest<ServerResponse<bool>>;


    public class CreateAccountHandler : IRequestHandler<CreateAccount, ServerResponse<bool>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CreateAccountHandler(DataContext applicationDbContext, IMapper mapper)
        {
            _dataContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<ServerResponse<bool>> Handle(CreateAccount request, CancellationToken cancellationToken)
        {
            if (await _dataContext.Users.AsNoTracking().AnyAsync(user => user.Email.ToLower()
                 .Equals(request.User.Email.ToLower())))
            {
                return new ServerResponse<bool>
                {
                    Data = false,
                   
                    Message = "User already exists."
                };
            }

            var user = _mapper.Map<User>(request.User);
            PasswordHash.Create(request.User.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;


            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            var result = _mapper.Map<UserDto>(user);

            return new ServerResponse<bool> { Data = true, Status = true, Message = "Registration successful!"};
        }

        

    }
}

