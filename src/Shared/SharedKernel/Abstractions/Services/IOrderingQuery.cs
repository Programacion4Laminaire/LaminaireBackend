﻿using SharedKernel.Commons.Bases;

namespace SharedKernel.Abstractions.Services
{
    public interface IOrderingQuery
    {
        IQueryable<T> Ordering<T>(BasePagination request, IQueryable<T> queryable) where T : class;
    }

}
