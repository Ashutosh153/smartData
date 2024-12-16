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
    public class AppointmentController : ControllerBase
    {
        public readonly IMediator _mediator;
        public AppointmentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("createAppointment")]
        public async Task<IActionResult> DoCreateAppointment(AddAppointmentDto addAppointment)
        {
            var result = await _mediator.Send(new CreateApponintmentCommand { AddAppointmentDto = addAppointment });
            return Ok(result);
        }

        [HttpPost("checkAvailableAppointment")]

        public async Task<IActionResult>DoVerifyAppointment(AddAppointmentDto varAppointment)
        {
            var result= await _mediator.Send(new VerifyAvailableAppointmentQuerry { verifyAppointment = varAppointment });
            return Ok(result);
        }

        [HttpGet("getAllAppointments/{id}")]

        public async Task<IActionResult>DoGetAllAppoints(int id)
        {
            var result= await _mediator.Send(new GetAllAppointmentsQuerry { Id = id });
            return Ok(result);
        }

        [HttpPost("completeAppointment/{id}")]
        public async Task<IActionResult>DoCompleteAppointment(int id)
        {
            var result= await _mediator.Send(new CompleteAppointmentCommand { AppointmentId = id });
            return Ok(result);  
        }

        [HttpPost("updateAppointment")]
        public async Task<IActionResult>DoUpdateAppointment(UpdateAppointmentDto updaetAppointment)
        {
            var result= await _mediator.Send(new UpdateAppointmentCommand { updateappointment = updaetAppointment });
            return Ok(result);
        }



    }
}
