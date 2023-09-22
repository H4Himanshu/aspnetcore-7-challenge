namespace Challenge.Contracts
{
    public interface IRepository<T>
    {
        /// <summary>Gets all asynchronous.</summary>
        /// <returns>Task&lt;List&lt;T&gt;&gt;.</returns>
        Task<List<T>> GetAllAsync();
        /// <summary>Gets the by identifier asynchronous.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        Task<T> GetByIdAsync(Guid id);
        /// <summary>Adds the asynchronous.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        Task<T> AddAsync(T entity);
        /// <summary>Updates the asynchronous.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        Task<T> UpdateAsync(Guid id, T entity);
        /// <summary>Deletes the asynchronous.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(Guid id);
    }
}
