using AEXMovies.Repositories.GenreRepository;
using AEXMovies.Services.GenreService;
using Microsoft.AspNetCore.Mvc;

namespace AEXMovies.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class GenresController : Controller
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> Search(string? q = null)
    {
        var genres = await _genreService.Search(q);
        return Ok(genres);
    }
    
}