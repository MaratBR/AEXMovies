using AEXMovies.Models;
using AEXMovies.Repositories;
using AEXMovies.Repositories.ActorRepository;
using AEXMovies.Repositories.GenreRepository;
using AEXMovies.Repositories.MovieRepository;
using AEXMovies.Services.Dtos;
using AEXMovies.Services.MovieService.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AEXMovies.Services.MovieService;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IActorRepository _actorRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IMapper _mapper;

    public MovieService(IMovieRepository movieRepository, 
                        IActorRepository actorRepository, 
                        IGenreRepository genreRepository,
                        IMapper mapper)
    {
        _movieRepository = movieRepository;
        _actorRepository = actorRepository;
        _genreRepository = genreRepository;
        _mapper = mapper;
    }
    
    public Task<MovieDetailsDto?> GetMovie(int id)
    {
        return _movieRepository.GetById<MovieDetailsDto>(id);
    }

    public async Task<MovieDetailsDto?> FindMovieByName(string fullName)
    {
        var list = await _movieRepository.FindMany<MovieDetailsDto>(m => m.Name == fullName, take: 1);
        return list.Count == 0 ? null : list[0];
    }

    public async Task<MovieDto> UpdateMovie(int id, UpdateMovieDto dto)
    {
        var movie = await _movieRepository.GetByIdOrThrow(id);

        if (dto.ActorIds != null)
            await _movieRepository.UpdateActors(movie, await FindActorsOrThrow(dto.ActorIds));

        if (dto.GenreIds != null)
            await _movieRepository.UpdateGenres(movie, await FindGenresOrThrow(dto.GenreIds));

        if (dto.Mpa != null)
            movie.Mpa = (MpaRating)dto.Mpa;
        if (dto.Name != null)
            movie.Name = dto.Name;
        
        await _movieRepository.Save(movie);
        return _mapper.Map<MovieDto>(movie);
    }

    public async Task DeleteById(int id)
    {
        var movie = await _movieRepository.GetByIdOrThrow(id);
        await _movieRepository.Delete(movie);
    }

    public async Task<List<MovieListItemDto>> SearchMovies(string? query, SearchOptions options)
    {
        if (query == null)
            return await _movieRepository.FindMany<MovieListItemDto>(take: 50);
        return await _movieRepository.FindMany<MovieListItemDto>(
            take: 50,
            filter: m => (options.SearchName && m.Name.Contains(query) ) || 
                         (options.SearchGenres && m.Genres.Any(g => g.Genre.NormalizedName.Contains(query))) || 
                         (options.SearchActors && m.Actors.Any(a => a.Actor.Name.Contains(query))));
    }

    public Task<List<MovieListItemDto>> SearchMovies(AdvancedSearchOptions options)
    {
        return _movieRepository.FindMany<MovieListItemDto>(
            filter: m => (options.NameQuery == null || m.Name.Contains(options.NameQuery)) &&
                         (options.ActorIds.Count == 0 || m.Actors.Any(a => options.ActorIds.Contains(a.ActorId))) &&
                         (options.GenreIds.Count == 0 || m.Genres.Any(g => options.GenreIds.Contains(g.GenreId))));
    }

    private async Task<List<Genre>> FindGenresOrThrow(ICollection<int> ids)
    {
        var genres = await _genreRepository.FindMany(g => ids.Contains(g.Id));
        if (genres.Count != ids.Count())
        {
            // some specified genres do not exist
            var invalidIds = ids.Except(genres.Select(a => a.Id));
            throw new InvalidGenreIds(invalidIds);
        }

        return genres;
    }
    
    private async Task<List<Actor>> FindActorsOrThrow(ICollection<int> ids)
    {
        var actors = await _actorRepository.FindMany(g => ids.Contains(g.Id));
        if (actors.Count != ids.Count())
        {
            // some specified genres do not exist
            var invalidIds = ids.Except(actors.Select(a => a.Id));
            throw new InvalidGenreIds(invalidIds);
        }

        return actors;
    }

    public async Task<MovieDto> CreateMovie(CreateNewMovieDto newMovieDto)
    {
        var genres = await FindGenresOrThrow(newMovieDto.GenreIds);
        var actors = await FindActorsOrThrow(newMovieDto.ActorIds);
        
        var movie = new Movie
        {
            Name = newMovieDto.Name,
            Mpa = newMovieDto.Mpa
        };

        await _movieRepository.Save(movie);
        await _movieRepository.UpdateGenres(movie, genres);
        await _movieRepository.UpdateActors(movie, actors);

        return _mapper.Map<MovieDto>(movie);
    }
}