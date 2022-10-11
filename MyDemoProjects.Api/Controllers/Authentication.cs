using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Features.Authentication.Queries;
using MyDemoProjects.Application.Shared.DTOs.Response;
using MyDemoProjects.Application.Shared.Models.Response;

namespace MyDemoProjects.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Authentication : Controller
    {
        private readonly ISender _mediator;

        public Authentication(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/createuser")]
        public async Task<ActionResult<ApplicationResponse<bool>>> CreateUser(CreateAccount newUser)
        {
            return await _mediator.Send(newUser);
        }

        [HttpPost("api/login")]
        public async Task<ActionResult<ApplicationResponse<TokenResponse>>> Login(LoginUser user)
        {
            return await _mediator.Send(user);
        }

        [HttpPost("api/changepassword")]
        public async Task<ActionResult<ApplicationResponse<bool>>> ChangePassword(ChangePassword user)
        {
            return await _mediator.Send(user);
        }

       
        [HttpPost("api/getallusers")]
        public async Task<ActionResult<ApplicationResponse<UserDetailsResponse>>> GetAllUsers()
        {
            return await _mediator.Send(new GetAllUsers());
        }
    }
}
