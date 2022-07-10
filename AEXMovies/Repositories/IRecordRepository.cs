using AEXMovies.Models;
using AEXMovies.Repositories.Exceptions;

namespace AEXMovies.Repositories;

public interface IRecordRepository<T> : IRepository<T> where T : RecordModel
{
    /// <summary>
    /// Gets the model with given id or null otherwise.
    /// </summary>
    /// <param name="id">Record ID</param>
    /// <returns>Task representing asynchronous operation.</returns>
    Task<T?> GetById(int id);
    
    /// <summary>
    /// Get the model with given id or throws an exception otherwise.
    /// </summary>
    /// <param name="id">Record ID</param>
    /// <exception cref="RecordNotFoundException">If record with given ID does not exist.</exception>
    /// <returns>Task representing asynchronous operation.</returns>
    Task<T> GetByIdOrThrow(int id);
    
    /// <summary>
    /// Gets deleted record with given id or null otherwise.
    /// </summary>
    /// <param name="id">Record ID</param>
    /// <returns>Task representing asynchronous operation.</returns>
    Task<T?> GetDeletedById(int id);
    
    /// <summary>
    /// Gets deleted record with given id or throws an exception otherwise.
    /// </summary>
    /// <param name="id">Record ID</param>
    /// <returns>Task whose value is the deleted record.</returns>
    Task<T> GetDeletedByIdOrThrow(int id);

    /// <summary>
    /// Gets record by ID and projects it to the given projection class.
    /// </summary>
    /// <param name="id">Record ID</param>
    /// <typeparam name="TProjection">Projection class.</typeparam>
    /// <returns>Task whose value is the projected deleted record or null.</returns>
    Task<TProjection?> GetById<TProjection>(int id);
    
    Task<bool> MarkAsDeleted(T model);
    
    Task<bool> UnmarkAsDeleted(T model);
}