using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Shared.DTO;
using MyDemoProjects.Application.Shared.DTOs.Request;
using MyDemoProjects.Application.Shared.DTOs.Response;

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
        public async Task<ActionResult<ApplicationResponse<bool>>> CreateUser(UserDto newUser)
        {
            return await _mediator.Send(new CreateAccount(newUser));
        }

        [HttpPost("api/login")]
        public async Task<ActionResult<ApplicationResponse<bool>>> Login(LoginUserRequest user)
        {
            return await _mediator.Send(new LoginUser(user));
        }
    }
}
