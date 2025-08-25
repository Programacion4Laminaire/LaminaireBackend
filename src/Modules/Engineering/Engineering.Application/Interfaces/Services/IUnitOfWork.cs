﻿using Engineering.Application.Interfaces.Persistence;
using System.Data;

namespace Engineering.Application.Interfaces.Services;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Product { get; }
    Task SaveChangesAsync();
    IDbTransaction BeginTransaction();
}
