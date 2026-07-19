using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrAccessSystem.Application.Features.QrCodes.Commands;

namespace QrAccessSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QrCodesController : ControllerBase
{
    private readonly IMediator _mediator;

    public QrCodesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateQr(GenerateQrCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (result.IsSuccess)
            return Ok(result);
            
        return BadRequest(result);
    }
}