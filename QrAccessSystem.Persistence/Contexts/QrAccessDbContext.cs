using Microsoft.EntityFrameworkCore;
using QrAccessSystem.Core.Common;
using QrAccessSystem.Core.Entities;

namespace QrAccessSystem.Persistence.Contexts;

public class QrAccessDbContext : DbContext
{
    public QrAccessDbContext(DbContextOptions<QrAccessDbContext> options) : base(options) { }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<QrCode> QrCodes { get; set; }
    public DbSet<AccessLog> AccessLogs { get; set; }
    public DbSet<Visitor> Visitors { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global Query Filter: Silinmiş verileri (Soft Delete) otomatik olarak sorgulardan gizle.
        modelBuilder.Entity<Department>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Employee>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Visitor>().HasQueryFilter(x => !x.IsDeleted);

        // Fluent API - Department & Employee İlişkisi
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict); // Departman silinirse personeller silinmesin.
        
        modelBuilder.Entity<QrCode>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<AccessLog>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<AppUser>().HasQueryFilter(x => !x.IsDeleted);
    }

    // Otomatik Audit Log Doldurma
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    // TODO: İleride JWT implemente edince UserID'yi buraya vereceğiz.
                    entry.Entity.CreatedBy = "System"; 
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = "System";
                    break;
                    
                case EntityState.Deleted:
                    entry.State = EntityState.Modified; // Gerçekten silme, Update yap.
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedDate = DateTime.UtcNow;
                    entry.Entity.DeletedBy = "System";
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}