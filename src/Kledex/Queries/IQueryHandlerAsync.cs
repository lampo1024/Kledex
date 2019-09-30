﻿using System.Threading.Tasks;

namespace Kledex.Queries
{
    public interface IQueryHandlerAsync<in TQuery, TResult> where TQuery : IQuery
    {
        Task<TResult> RetrieveAsync(TQuery query);
    }

    public interface IQueryHandlerAsync2<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
