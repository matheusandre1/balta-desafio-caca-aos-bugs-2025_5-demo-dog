using BugStore.Domain.Base;
using BugStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Infrastructure.Data;
public class Repository<T>(AppDbContext _context) : IRepository<T> where T : class
{     

    public async Task AddAsync(T entity)
    {
        _context.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        var entity = await _context.Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e=> EF.Property<Guid>(e, "Id") == id);

        return entity!;
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Set<T>()
            .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);

        _context.Set<T>().Remove(entity!);
        await _context.SaveChangesAsync();
    }   
}