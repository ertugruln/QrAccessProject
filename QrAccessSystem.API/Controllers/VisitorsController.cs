using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrAccessSystem.Application.Features.Visitors.Commands;
using Microsoft.AspNetCore.Authorization;

namespace QrAccessSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VisitorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public VisitorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVisitorCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (result.IsSuccess)
            return Ok(result);
            
        return BadRequest(result);
    }
}   