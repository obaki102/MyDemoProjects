using Microsoft.AspNetCore.Mvc;
using MyDemoProjects.Server.Application.Features.Authentication.Command;
using MyDemoProjects.Shared;
using MyDemoProjects.Shared.DTO;
using MyDemoProjects.Shared.DTO.Response;

namespace MyDemoProjects.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ISender _mediator;

        public WeatherForecastController(ISender mediator)
        {
            _mediator = mediator;
        }
    
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

       

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpPost]
        public async Task<ActionResult<ServerResponse<UserDto>>> RegisterUser(UserDto userDto)
        {

            return await _mediator.Send(new RegisterUserCommand(userDto));
        }
    }
}