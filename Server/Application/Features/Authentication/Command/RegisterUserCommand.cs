using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyDemoProjects.Server.Application.Features.Authentication.Utility;
using MyDemoProjects.Server.Data;
using MyDemoProjects.Server.Domain.Entities;
using MyDemoProjects.Shared.DTO;
using MyDemoProjects.Shared.DTO.Response;
using System.Security.Cryptography;

namespace MyDemoProjects.Server.Application.Features.Authentication.Command
{
    public record RegisterUserCommand(UserDto User) : IRequest<ServerResponse<UserDto>>;


    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, ServerResponse<UserDto>>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public RegisterUserHandler(DataContext applicationDbContext, IMapper mapper)
        {
            _dataContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<ServerResponse<UserDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _dataContext.Users.AnyAsync(user => user.Email.ToLower()
                 .Equals(request.User.Email.ToLower())))
            {
                return new ServerResponse<UserDto>
                {
                    Status = false,
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

            return new ServerResponse<UserDto> { Data = result, Message = "Registration successful!" };
        }

        

    }
}

