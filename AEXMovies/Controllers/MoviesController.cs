using AEXMovies.Repositories.Exceptions;
using AEXMovies.Services.Dtos;
using AEXMovies.Services.MovieService;
using AEXMovies.Services.MovieService.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AEXMovies.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class MoviesController : Controller
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string? q = null, bool byActors = true, bool byGenre = true,
        bool byName = true)
    {
        var movies = await _movieService.SearchMovies(q, new SearchOptions
        {
            SearchActors = byActors,
            SearchGenres = byGenre,
            SearchName = byName
        });
        return Ok(movies);
    }

    [HttpGet("search/advanced")]
    public async Task<IActionResult> Search([FromQuery] AdvancedSearchOptions options)
    {
        var movies = await _movieService.SearchMovies(options);
        return Ok(movies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var movie = await _movieService.GetMovie(id);

        if (movie == null)
            return NotFound(new { Message = $"Movie with ID = {id} not found" });
        return Ok(movie);
    }

    [Authorize(Roles = "Admin,Moderator")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateNewMovieDto body)
    {
        try
        {
            return Ok(await _movieService.CreateMovie(body));
        }
        catch (MovieException exc)
        {
            return UnprocessableEntity(exc.Message);
        }
    }

    [Authorize(Roles = "Admin,Moderator")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, UpdateMovieDto body)
    {
        try
        {
            return Ok(await _movieService.UpdateMovie(id, body));
        }
        catch (RecordNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (MovieException e)
        {
            return UnprocessableEntity(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _movieService.DeleteById(id);
            return Ok();
        }
        catch (RecordNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}