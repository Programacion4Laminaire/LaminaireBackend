using Microsoft.EntityFrameworkCore;
using SGI.Application.Interfaces.Persistence;
using SGI.Domain.Entities;
using SGI.Infrastructure.Persistence.Context;
using SharedKernel.Abstractions.Services;
namespace SGI.Infrastructure.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _entity;
    private readonly ICurrentUserService _currentUser; // 👈 servicio inyectado

    public GenericRepository(ApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _entity = _context.Set<T>();
        _currentUser = currentUser;
    }

    public IQueryable<T> GetAllQueryable()
    {
        return _entity.Where(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _entity
            .Where(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null)
            .ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        var response = await _entity
            .SingleOrDefaultAsync(x => x.Id == id && x.AuditDeleteUser == null && x.AuditDeleteDate == null);

        return response!;
    }

    public async Task CreateAsync(T entity)
    {
        entity.AuditCreateUser = _currentUser.UserId ?? 1; // 👈 usuario logueado o 1 si no hay contexto
        entity.AuditCreateDate = DateTime.UtcNow; // recomendable usar UTC

        await _context.AddAsync(entity);
    }

    public void UpdateAsync(T entity)
    {
        entity.AuditUpdateUser = _currentUser.UserId ?? 1;
        entity.AuditUpdateDate = DateTime.UtcNow;

        _context.Update(entity);

        // Evita modificar la auditoría de creación
        _context.Entry(entity).Property(x => x.AuditCreateUser).IsModified = false;
        _context.Entry(entity).Property(x => x.AuditCreateDate).IsModified = false;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);

        entity.AuditDeleteUser = _currentUser.UserId ?? 1;
        entity.AuditDeleteDate = DateTime.UtcNow;

        _context.Update(entity);
    }
}