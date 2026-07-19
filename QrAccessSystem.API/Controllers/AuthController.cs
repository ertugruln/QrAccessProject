using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrAccessSystem.Application.Features.Auth.Commands;

namespace QrAccessSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess) return Ok(result);
        return BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsSuccess) return Ok(result);
        return BadRequest(result);
    }
}