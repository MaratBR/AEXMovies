using System.Linq.Expressions;

namespace AEXMovies.Repositories;

public interface IRepository<T>
{
    /// <summary>
    /// FindMany searches for all record in the database that satisfy given filter predicate.
    /// If predicate is null, returns all records.
    /// </summary>
    /// <param name="filter">Filtering predicate</param>
    /// <param name="skip">How many records to skip (equivalent of SQL OFFSET)</param>
    /// <param name="take">How many records to take (equivalent of SQL LIMIT/TOP)</param>
    /// <returns>Task representing a list of records</returns>
    Task<List<T>> FindMany(
        Expression<Func<T, bool>>? filter = null, 
        int? skip = null, 
        int? take = null);
    
    Task<List<TProjection>> FindMany<TProjection>(
        Expression<Func<T, bool>>? filter = null, 
        int? skip = null, 
        int? take = null);
    
    /// <summary>
    /// Orders the records that satisfy filter predicate (if given) and returns them.
    /// </summary>
    /// <param name="orderBy">Function expression that returns the field for ordering</param>
    /// <param name="orderByDescending">If set to true, will sort in descending order</param>
    /// <param name="filter">Filtering predicate</param>
    /// <param name="skip">How many records to skip (equivalent of SQL OFFSET)</param>
    /// <param name="take">How many records to take (equivalent of SQL LIMIT/TOP)</param>
    /// <typeparam name="TOrderBy">Type of field that sequence will be ordered by</typeparam>
    /// <returns>Task representing a list of records</returns>
    Task<List<T>> FindAndOrderMany<TOrderBy>(
        Expression<Func<T, TOrderBy>> orderBy,
        Expression<Func<T, bool>>? filter = null,
        bool orderByDescending = false,
        int? skip = null, 
        int? take = null);
    
    Task<List<TProjection>> FindAndOrderMany<TProjection, TOrderBy>(
        Expression<Func<TProjection, TOrderBy>> orderBy,
        Expression<Func<T, bool>>? filter = null,
        bool orderByDescending = false,
        int? skip = null, 
        int? take = null);


    Task<T?> FindOne(Expression<Func<T, bool>> filter);
    Task<TProjection?> FindOne<TProjection>(Expression<Func<T, bool>> filter);
    
    Task Delete(T record);
    Task Save(T record);
    Task Save(IEnumerable<T> records);
}