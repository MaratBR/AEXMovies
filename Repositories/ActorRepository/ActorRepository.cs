using System.Linq.Expressions;
using AEXMovies.Models;
using AutoMapper;

namespace AEXMovies.Repositories.ActorRepository;

public class ActorRepository : EfRecordRepository<Actor>, IActorRepository
{
    private readonly EfDbContext _context;
    private readonly IMapper _mapper;
    
    public ActorRepository(EfDbContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
        _context = context;
    }
}