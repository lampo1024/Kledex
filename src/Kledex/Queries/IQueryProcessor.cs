﻿using System.Threading.Tasks;

namespace Kledex.Queries
{
    /// <summary>
    /// IQueryProcessor
    /// </summary>
    public interface IQueryProcessor
    {
        /// <summary>
        /// Asynchronously gets the result.
        /// The query handler must implement Kledex.Queries.IQueryHandlerAsync&lt;TQuery, TResult&gt;.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>TResult</returns>
        Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query);

        /// <summary>
        /// Gets the result.
        /// The query handler must implement Kledex.Queries.IQueryHandler&lt;TQuery, TResult&gt;.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>TResult</returns>
        TResult Process<TResult>(IQuery<TResult> query);
    }
}
