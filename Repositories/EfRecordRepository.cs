using AEXMovies.Models;
using AEXMovies.Repositories.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AEXMovies.Repositories;

public class EfRecordRepository<T> : EfRepository<T>, IRecordRepository<T> where T : RecordModel
{
    public EfRecordRepository(EfDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public Task<T?> GetById(int id)
    {
        return FindOne(r => r.Id == id && r.DeletedAt == null);
    }

    public Task<T> GetByIdOrThrow(int id)
    {
        var model = GetById(id);
        if (model == null)
            throw new RecordNotFoundException(typeof(T), id);
        return model!;
    }

    public Task<T?> GetDeletedById(int id)
    {
        return FindOne(r => r.Id == id && r.DeletedAt != null);
    }

    public Task<T> GetDeletedByIdOrThrow(int id)
    {
        var model = GetDeletedById(id);
        if (model == null)
            throw new RecordNotFoundException(typeof(T), id);
        return model!;    }

    public Task<TProjection?> GetById<TProjection>(int id)
    {
        return FindOne<TProjection>(r => r.Id == id);
    }

    public async Task<bool> MarkAsDeleted(T model)
    {
        if (model.DeletedAt != null)
            return false;
        model.DeletedAt = DateTime.Now;
        await Save(model);
        return true;
    }

    public async Task<bool> UnmarkAsDeleted(T model)
    {
        if (model.DeletedAt == null)
            return false;
        model.DeletedAt = null;
        await Save(model);
        return false;
    }
}