using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QrAccessSystem.Application.Interfaces;
using QrAccessSystem.Core.Common;
using QrAccessSystem.Persistence.Contexts;

namespace QrAccessSystem.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseAuditableEntity
{
    protected readonly QrAccessDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(QrAccessDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity); // EF Core DbContext'te override ettiğimiz için aslında IsDeleted = true yapacak.
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}