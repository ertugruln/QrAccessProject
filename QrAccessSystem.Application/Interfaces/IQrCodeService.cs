namespace QrAccessSystem.Application.Interfaces;

public interface IQrCodeService
{
    string GenerateSecurePayload(Guid employeeId, string uniqueToken);
    
    // YENİ EKLENEN METOT
    bool ValidateSignature(string payload);
}