using QrAccessSystem.Core.Common;

namespace QrAccessSystem.Core.Entities;

public class QrCode : BaseAuditableEntity
{
    // QR kod okutulduğunda doğrulanacak şifreli ve benzersiz veri
    public string Payload { get; set; } = string.Empty; 
    
    // QR kodun geçerlilik süresi (Örn: Üretildikten sonraki 1 dakika)
    public DateTime ExpireDate { get; set; } 
    
    // Güvenlik riski durumunda QR kodu manuel iptal etme durumu
    public bool IsRevoked { get; set; } = false;

    // Hangi personelin QR kodu?
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
}