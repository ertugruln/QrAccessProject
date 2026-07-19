using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QrAccessSystem.Application.Interfaces;
using QrAccessSystem.Core.Entities;

namespace QrAccessSystem.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        // HMACSHA512 algoritması ile şifreyi geri döndürülemez şekilde hash'liyoruz
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        // Veritabanındaki "Tuz (Salt)" ile girilen şifreyi tekrar hash'leyip karşılaştırıyoruz
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    public string CreateToken(AppUser user)
    {
        // Token'ın içine koyacağımız bilgiler (Payload / Claims)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()) // Kullanıcının yetkisi (Admin, Güvenlik vb.)
        };

        // appsettings.json dosyasından gizli anahtarımızı (Secret Key) alıyoruz
        var tokenKey = _configuration.GetSection("AppSettings:Token").Value;
        if (string.IsNullOrEmpty(tokenKey))
            throw new Exception("Token gizli anahtarı bulunamadı!");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); // İmzayı atıyoruz

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1), // Token 1 gün boyunca geçerli olacak
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token); // Üretilen token'ı string (metin) olarak dönüyoruz
    }
}