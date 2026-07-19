using MediatR;
using QrAccessSystem.Application.Wrappers;

namespace QrAccessSystem.Application.Features.Auth.Commands;

// Başarılı olursa geriye string tipinde bir JWT Token dönecek
public class LoginCommand : IRequest<Response<string>>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}