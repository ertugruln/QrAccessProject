using MediatR;
using QrAccessSystem.Application.Wrappers;

namespace QrAccessSystem.Application.Features.QrCodes.Commands;

// Personel kendi ID'sini gönderip QR metnini isteyecek
public class GenerateQrCommand : IRequest<Response<string>>
{
    public Guid EmployeeId { get; set; }
}