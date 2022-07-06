using AEXMovies.Models;
using AutoMapper;

namespace AEXMovies.Repositories.GenreRepository;

public class GenreRepository : EfRecordRepository<Genre>, IGenreRepository
{
    public GenreRepository(EfDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}