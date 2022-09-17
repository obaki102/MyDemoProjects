using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDemoProjects.Server.Application.Features.Authentication.Command;

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
        [HttpPost("api/registerUser")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ServerResponse<bool>>> RegisterUser(UserDto user)
        {
            return await _mediator.Send(new RegisterUser(user));
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
