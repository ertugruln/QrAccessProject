using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrAccessSystem.Application.Features.Access.Commands;

namespace QrAccessSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccessController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccessController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("scan")]
    public async Task<IActionResult> ScanQr(ScanQrCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (result.IsSuccess)
            return Ok(result);
            
        // İş kurallarından (süre dolması vs.) kalan hataları da 400 döneriz
        return BadRequest(result);
    }
}