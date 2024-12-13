using App.Core.App.Command;
using App.Core.App.Query;
using App.Core.ModelsDto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHRApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("createUserController")]
        public async Task<IActionResult> CreateUser(RegModelDto reg)
        {
            var result=await _mediator.Send(new CreateUserCommand { newUser = reg });
            return Ok(result);
        }
        [HttpGet("getAllStates")]

        public async Task<IActionResult> getAllStates()
        {
            var result = await _mediator.Send(new GetAllStatesQuery { });
            return Ok(result);

        }
        [HttpGet("getAllCities/{id}")]
        public async Task<IActionResult> getAllStates(int id)
        {
            var result = await _mediator.Send(new GetAllCitiesQuerry { id = id });
            return Ok(result);
        }
        [HttpGet("getAllUsers/{id}")]
        public async Task<IActionResult> GetAllUsers(int id)
        {
            var result=await _mediator.Send(new GetAllUsersQuery { userId = id });
            return Ok(result);
        }
        
    }
}
