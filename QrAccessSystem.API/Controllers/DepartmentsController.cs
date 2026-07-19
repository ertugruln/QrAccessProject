using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrAccessSystem.Application.Features.Departments.Commands;

namespace QrAccessSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateDepartmentCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (result.IsSuccess)
            return Ok(result);
            
        return BadRequest(result);
    }
}