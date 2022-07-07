using AEXMovies.Models;

namespace AEXMovies.Repositories;

public interface IRecordRepository<T> : IRepository<T> where T : RecordModel
{
    Task<T?> GetById(int id);
    Task<T> GetByIdOrThrow(int id);
    Task<T?> GetDeletedById(int id);
    Task<T> GetDeletedByIdOrThrow(int id);

    Task<TProjection?> GetById<TProjection>(int id);
    Task<bool> MarkAsDeleted(T model);
    Task<bool> UnmarkAsDeleted(T model);
}