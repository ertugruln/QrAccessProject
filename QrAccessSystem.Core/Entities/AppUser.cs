using QrAccessSystem.Core.Common;
using QrAccessSystem.Core.Enums;

namespace QrAccessSystem.Core.Entities;

public class AppUser : BaseAuditableEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Şifreyi açık tutmuyoruz! Kriptografik özet (Hash) ve Tuz (Salt) kullanacağız.
    public byte[] PasswordHash { get; set; } = new byte[0];
    public byte[] PasswordSalt { get; set; } = new byte[0];

    public UserRole Role { get; set; }

    public bool IsActive { get; set; } = true;
}