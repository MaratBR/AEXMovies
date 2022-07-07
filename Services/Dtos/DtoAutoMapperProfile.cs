using AEXMovies.Models;

namespace AEXMovies.Services.Dtos;

public class DtoAutoMapperProfile : AutoMapper.Profile
{
    public DtoAutoMapperProfile()
    {
        CreateMap<Movie, MovieDto>();
        CreateMap<Movie, MovieListItemDto>()
            .ForMember(dto => dto.Genres, o => o.MapFrom(m => m.Genres.Select(g => g.Genre)));
        CreateMap<Movie, MovieDetailsDto>()
            .ForMember(
                dto => dto.Actors,
                options => options.MapFrom(m => m.Actors.Select(a => a.Actor)))
            .ForMember(dto => dto.Genres, o => o.MapFrom(m => m.Genres.Select(g => g.Genre)));
        CreateMap<Actor, ActorDto>();
        CreateMap<Actor, ActorDetailsDto>()
            .ForMember(
                dto => dto.Movies,
                o => o.MapFrom(a => a.Movies.Select(m => m.Movie)));
        CreateMap<Genre, GenreDto>();
    }
}