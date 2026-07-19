using MediatR;
using QrAccessSystem.Application.Wrappers;
using QrAccessSystem.Core.Enums;

namespace QrAccessSystem.Application.Features.Auth.Commands;

public class RegisterCommand : IRequest<Response<Guid>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // Kullanıcının girdiği açık şifre
    public UserRole Role { get; set; } // Admin mi, Güvenlik mi?
}