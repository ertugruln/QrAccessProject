using MediatR;
using QrAccessSystem.Application.Wrappers;

namespace QrAccessSystem.Application.Features.Visitors.Commands;

public class CreateVisitorCommand : IRequest<Response<Guid>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdentityNumber { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string VisitPurpose { get; set; } = string.Empty;

    public DateTime ExpectedArrival { get; set; }
    public DateTime ExpectedDeparture { get; set; }

    // Kimi ziyarete geldi?
    public Guid HostEmployeeId { get; set; }
}