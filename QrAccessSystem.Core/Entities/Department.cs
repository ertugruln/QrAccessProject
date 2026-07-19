using QrAccessSystem.Core.Common;

namespace QrAccessSystem.Core.Entities;

public class Department : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    // Navigation Property
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}