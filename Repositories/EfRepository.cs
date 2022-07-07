using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AEXMovies.Repositories;

public class EfRepository<TModel> : IRepository<TModel> where TModel : class
{
    private readonly EfDbContext _context;
    private readonly DbSet<TModel> _dbSet;
    private readonly IMapper _mapper;

    public EfRepository(EfDbContext context, IMapper mapper)
    {
        _context = context;
        _dbSet = context.Set<TModel>();
        _mapper = mapper;
    }

    private IQueryable<TModel> Query(Expression<Func<TModel, bool>>? filter = null, 
                                    int? skip = null, int? take = null)
    {
        var queryable = filter == null ? _dbSet : _dbSet.Where(filter);

        if (skip != null)
        {
            if ((int)skip < 0)
                throw new ArgumentException("offset (skip) value is negative", nameof(skip));
            queryable = queryable.Skip((int)skip);
        }

        if (take != null)
        {
            if ((int)take < 0)
                throw new ArgumentException("take (limit) value is negative", nameof(take));
            queryable = queryable.Take((int)take);
        }

        return queryable;
    }

    public Task<List<TModel>> FindMany(Expression<Func<TModel, bool>>? filter = null, int? skip = null, int? take = null)
    {
        return Query(filter, skip, take).ToListAsync();
    }

    public Task<List<TProjection>> FindMany<TProjection>(Expression<Func<TModel, bool>>? filter = null, int? skip = null, int? take = null)
    {
        return Query(filter, skip, take).ProjectTo<TProjection>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public Task<List<TModel>> FindAndOrderMany<TOrderBy>(
        Expression<Func<TModel, TOrderBy>> orderBy, 
        Expression<Func<TModel, bool>>? filter = null,
        bool orderByDescending = false,
        int? skip = null, 
        int? take = null)
    {
        var q = Query(filter, skip, take);
        return (orderByDescending ? q.OrderByDescending(orderBy) : q.OrderBy(orderBy)).ToListAsync();
    }
    
    public Task<List<TProjection>> FindAndOrderMany<TProjection, TOrderBy>(
        Expression<Func<TProjection, TOrderBy>> orderBy, 
        Expression<Func<TModel, bool>>? filter = null,
        bool orderByDescending = false,
        int? skip = null, 
        int? take = null)
    {
        var q = Query(filter, skip, take).ProjectTo<TProjection>(_mapper.ConfigurationProvider);
        return (orderByDescending ? q.OrderByDescending(orderBy) : q.OrderBy(orderBy)).ToListAsync();
    }

    public Task<TModel?> FindOne(Expression<Func<TModel, bool>> filter)
    {
        // Will throw error if more than one record is found
        return Query(filter).SingleOrDefaultAsync();
    }

    public Task<TProjection?> FindOne<TProjection>(Expression<Func<TModel, bool>> filter, Expression<Func<TModel, TProjection>> projection)
    {
        return Query(filter).Select(projection).SingleOrDefaultAsync();
    }
    
    public Task<TProjection?> FindOne<TProjection>(Expression<Func<TModel, bool>> filter)
    {
        return Query(filter).ProjectTo<TProjection>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public virtual async Task Delete(TModel record)
    {
        _dbSet.Remove(record);
        await _context.SaveChangesAsync();
    }

    public virtual async Task Save(TModel record)
    {
        if (_context.Entry(record).State == EntityState.Detached)
        {
            _dbSet.Add(record);
        }
        await _context.SaveChangesAsync();
    }

    public virtual async Task Save(IEnumerable<TModel> records)
    {
        _dbSet.AddRange(records.Where(r => _context.Entry(r).State == EntityState.Detached));
        await _context.SaveChangesAsync();
    }
}