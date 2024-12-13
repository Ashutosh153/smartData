using App.Core.App.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EHRApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveImageController : ControllerBase
    {
        public readonly IMediator _mediator;
        public SaveImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("saveImage")]
        public async Task<IActionResult> DoUploadFile(IFormFile file)
        {
            var result = await _mediator.Send(new SaveImageCommand { imageFile = file });
            return Ok(result);
        }
    }
}
