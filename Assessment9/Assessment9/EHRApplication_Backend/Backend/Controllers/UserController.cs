using App.Core.App.Command;
using App.Core.App.Query;
using App.Core.ModelsDto;
using Domain.Models.User;
using Infrastructure.Migrations;
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
            var result = await _mediator.Send(new CreateUserCommand { newUser = reg });
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
            var result = await _mediator.Send(new GetAllUsersQuery { userId = id });
            return Ok(result);
        }
        [HttpGet("getAllSpecilizations")]
        public async Task<IActionResult> GetAllSpecilizations()
        {
            var result = await _mediator.Send(new GetAllSpecilizationQuery());
            return Ok(result);
        }

        [HttpPost("DoVerifyCredentials")]
        public async Task<IActionResult> DoverifyCredentials(LoginDto login)
        {
            var result = await _mediator.Send(new LoginQuery { Login = login });
            return Ok(result);
        }
        [HttpPost("varifyOtpAndLogin")]
        public async Task<IActionResult> DoVerifyOtp(Otp otp)
        {
            var result = await _mediator.Send(new VerifyOtpQuery { Otp = otp });
            return Ok(result);
        }

        [HttpPost("forgetPassword")]
        public async Task<IActionResult> DoForgetPassword(string email)
        {
            var result = await _mediator.Send(new ForgetPasswordCommand { Email = email });
            return Ok(result);
        }

        [HttpGet("getUserByID/{id}")]
        public async Task<IActionResult>DoGetUserById(int id)
        {
            var result=await _mediator.Send(new GetUserByIdQuerry { id = id });
            return Ok(result);
        }
        [HttpPost("updatePatientController")]
        public async Task<IActionResult>DoUpdatePatient(UpdatePatientDto updatePatient)
        {
            var result=await _mediator.Send(new UpdatePatientCommand { UpdatePatientDto = updatePatient });
            return Ok(result);
        }
        [HttpPost("updateProviderCommand")]
        public async Task<IActionResult>DoUpdateProvider(UpdateProviderDto updateProvider)
        {
            var result= await _mediator.Send(new UpdateProviderCommand { providerDto = updateProvider });
            return Ok(result);
        }
      
        [HttpGet("getPractionersAsRequired/{id}")]
        public async Task<IActionResult>DoGetPractionersAsRequired(int id)
        {
            var result=await _mediator.Send(new GetAllPractionerQuerry { specilizationId = id });
            return Ok(result);
        }
        [HttpPost("CancelAppointment/{id}")]
        public async Task<IActionResult>DoCancelAppointment(int id)
        {
            var result=await _mediator.Send(new CancelAppointmentCommand { AppointmentId = id });
            return Ok(result);
        }
        [HttpPost("CompleteAppointment/{id}")]
        public async Task<IActionResult>DoCompleteAppointment(int id)
        {
            var result= await _mediator.Send(new CompleteAppointmentCommand { AppointmentId = id });
            return Ok(result);
        }
        [HttpPost("changePassword")]
        public async Task<IActionResult>DoChangePassword(ChangePasswordDto changepass)
        {
            var result=await _mediator.Send(new ChangePasswordCommand { chnagePassObj = changepass });
            return Ok(result);
        }
    }
}
