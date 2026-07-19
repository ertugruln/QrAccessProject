using MediatR;
using QrAccessSystem.Application.Wrappers;
using QrAccessSystem.Core.Enums;

namespace QrAccessSystem.Application.Features.Access.Commands;

public class ScanQrCommand : IRequest<Response<string>>
{
    public string Payload { get; set; } = string.Empty;
    
    // 1: Giriş Turnikesi, 2: Çıkış Turnikesi
    public AccessType AccessType { get; set; } 
}