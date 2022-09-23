using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyDemoProjects.Server.Application.Features.Authentication.Utility;
using MyDemoProjects.Server.Data;
using MyDemoProjects.Server.Domain.Entities;
using System.Security.Cryptography;

namespace MyDemoProjects.Server.Application.Features.Authentication.Commands
{
    public record CreateAccount(UserDto User1) : IRequest<ServerResponse<bool>>;


    public class CreateAccountHandler : IRequestHandler<CreateAccount, ServerResponse<bool>>
    {
        private readonly Data.ApplicationDbContext _dataContext;
        private readonly IMapper _mapper;

        public CreateAccountHandler(Data.ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _dataContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<ServerResponse<bool>> Handle(CreateAccount request, CancellationToken cancellationToken)
        {
            if (await _dataContext.Users.AsNoTracking().AnyAsync(User1 => User1.Email.ToLower()
                 .Equals(request.User1.Email.ToLower())))
            {
                return new ServerResponse<bool>
                {
                    Data = false,
                   
                    Message = "User1 already exists."
                };
            }

            var User1 = _mapper.Map<User1>(request.User1);
            PasswordHash.Create(request.User1.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User1.PasswordHash = passwordHash;
            User1.PasswordSalt = passwordSalt;


            //_dataContext.Users.Add(User1);
            await _dataContext.SaveChangesAsync();

            var result = _mapper.Map<UserDto>(User1);

            return new ServerResponse<bool> { Data = true, Status = true, Message = "Registration successful!"};
        }

        

    }
}

