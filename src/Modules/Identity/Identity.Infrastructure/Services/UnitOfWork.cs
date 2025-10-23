using Identity.Application.Interfaces.Persistence;
using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence.Context;
using Identity.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernel.Abstractions.Services; // 👈 importante
using System.Data;

namespace Identity.Infrastructure.Services
{
    public class UnitOfWork(ApplicationDbContext context, ICurrentUserService currentUser) : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ICurrentUserService _currentUser = currentUser; // 👈 inyectado

        private IUserRepository? _user;
        private IMenuRepository? _menu;
        private IGenericRepository<Role>? _role;
        private IGenericRepository<UserRole>? _userRole;
        private IPermissionRepository? _permission;
        private IRefreshTokenRepository? _refreshToken;

        public IUserRepository User => _user ??= new UserRepository(_context, _currentUser);
        // UnitOfWork.cs (línea clave)
        public IMenuRepository Menu => _menu ??= new MenuRepository(_context, _currentUser);

        public IGenericRepository<Role> Role => _role ??= new GenericRepository<Role>(_context, _currentUser);
        public IGenericRepository<UserRole> UserRole => _userRole ??= new GenericRepository<UserRole>(_context, _currentUser);
        public IPermissionRepository Permission => _permission ??= new PermissionRepository(_context);
        public IRefreshTokenRepository RefreshToken => _refreshToken ??= new RefreshTokenRepository(_context);
       

        public IDbTransaction BeginTransaction()
        {
            var transaction = _context.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
