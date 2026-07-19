using System.Security.Cryptography;
using System.Text;
using QrAccessSystem.Application.Interfaces;

namespace QrAccessSystem.Infrastructure.Services;

public class QrCodeService : IQrCodeService
{
    private const string SecretKey = "SuperSecretEnterpriseQrAccessKey!!!";

    public string GenerateSecurePayload(Guid employeeId, string uniqueToken)
    {
        var rawData = $"{employeeId}:{uniqueToken}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        var signature = Convert.ToBase64String(hashBytes);
        return $"{rawData}:{signature}";
    }

    // YENİ EKLENEN METOT: İmza Doğrulama
    public bool ValidateSignature(string payload)
    {
        var parts = payload.Split(':');
        if (parts.Length != 3) return false; // Format hatalıysa direkt reddet

        var rawData = $"{parts[0]}:{parts[1]}";
        var providedSignature = parts[2]; // Bize gelen imza

        // Aynı veriyle biz yeniden imza üretiyoruz
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        var computedSignature = Convert.ToBase64String(hashBytes);

        // Bizim ürettiğimiz imza ile gelen imza aynıysa bu QR'ı BİZ üretmişizdir.
        return providedSignature == computedSignature;
    }
}