using QrAccessSystem.Core.Common;
using QrAccessSystem.Core.Enums;

namespace QrAccessSystem.Core.Entities;

public class AccessLog : BaseAuditableEntity
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public Guid QrCodeId { get; set; }
    public QrCode QrCode { get; set; } = null!;

    public AccessType AccessType { get; set; } // Giriş mi? Çıkış mı?
    public DateTime AccessTime { get; set; } // İşlemin tam zamanı
    
    // Başarılı mı yoksa sahte QR veya geçersiz saat sebebiyle reddedildi mi?
    public bool IsSuccess { get; set; } 
    public string? FailureReason { get; set; } 
}