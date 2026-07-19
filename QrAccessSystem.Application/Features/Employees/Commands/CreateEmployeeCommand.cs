using MediatR;
using QrAccessSystem.Application.Wrappers;

namespace QrAccessSystem.Application.Features.Employees.Commands;

public class CreateEmployeeCommand : IRequest<Response<Guid>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    
    // Vardiya saatlerini "HH:mm:ss" formatında text olarak alacağız
    public TimeSpan ShiftStartTime { get; set; }
    public TimeSpan ShiftEndTime { get; set; }
    
    // Personelin atanacağı departmanın ID'si
    public Guid DepartmentId { get; set; }
}