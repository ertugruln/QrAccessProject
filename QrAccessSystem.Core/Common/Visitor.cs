using QrAccessSystem.Core.Common;

namespace QrAccessSystem.Core.Entities;

public class Visitor : BaseAuditableEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdentityNumber { get; set; } = string.Empty; // TC Kimlik / Pasaport
    public string CompanyName { get; set; } = string.Empty;
    public string VisitPurpose { get; set; } = string.Empty;

    // Randevu Saatleri
    public DateTime ExpectedArrival { get; set; }
    public DateTime ExpectedDeparture { get; set; }

    // Güvenlik veya ziyaret edilen personel onayladı mı?
    public bool IsApproved { get; set; } = false;

    // Ziyaretçi kimi görmeye geldi? (Ev Sahibi Personel)
    public Guid HostEmployeeId { get; set; }
    public Employee HostEmployee { get; set; } = null!;
}