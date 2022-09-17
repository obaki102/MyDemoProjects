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
        public async Task<ActionResult<ServerResponse<bool>>> RegisterUser(UserDto userDto)
        {
            return await _mediator.Send(new RegisterUser(userDto));
        }


        [HttpPost("api/changePassword")]
        public async Task<ActionResult<ServerResponse<bool>>> ChangePassword(ChangePasswordRequest request)
        {
            return await _mediator.Send(new ChangePassword(request));
        }
    }

}
