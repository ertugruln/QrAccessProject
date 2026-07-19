using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrAccessSystem.Application.Features.Employees.Commands;

namespace QrAccessSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEmployeeCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (result.IsSuccess)
            return Ok(result);
            
        return BadRequest(result);
    }
}