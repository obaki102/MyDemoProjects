using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDemoProjects.Server.Application.Features.AnimeList.Commands;
using MyDemoProjects.Server.Application.Features.AnimeList.Queries;
using MyDemoProjects.Server.Application.Features.Authentication.Commands;

namespace MyDemoProjects.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {

        private readonly ISender _mediator;

        public AuthenticationController(ISender mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("api/createAccount")]
        public async Task<ActionResult<ServerResponse<bool>>> CreateAccount(UserDto user)
        {
            return await _mediator.Send(new CreateAccount(user));
        }

        [HttpPost("api/loginUser")]
        public async Task<ActionResult<ServerResponse<string>>> Login(LoginUserRequest user)
        {
            return await _mediator.Send(new LoginUser(user));
        }


        [HttpPost("api/changePassword")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServerResponse<bool>>> ChangePassword(ChangePasswordRequest request)
        {
            return await _mediator.Send(new ChangePassword(request));
        }


    }

}
