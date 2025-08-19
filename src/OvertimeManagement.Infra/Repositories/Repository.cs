using Microsoft.EntityFrameworkCore;
using OvertimeManagement.Domain.Interfaces;
using OvertimeManagement.Domain.Models;
using OvertimeManagement.Infra.Context;

namespace OvertimeManagement.Infra.Repositories;

public class Repository<TEntity>(AppDbContext context) : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly AppDbContext _context = context;

    public virtual Task<TEntity?> GetByIdAsync(int id)
    {
        return _context.Set<TEntity>().FindAsync(id).AsTask();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task SaveAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task RemoveAsync(int id)
    {
        var entity = await GetByIdAsync(id);

        if (entity != null)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
