using AEXMovies.Services.AssetsLoaderService;
using AEXMovies.Services.Dtos;
using AEXMovies.Services.GenreService;
using AEXMovies.Services.MovieService;

namespace AEXMovies;

public static class Populate
{
    public static async Task Run(WebApplication app)
    {
        using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var assetsLoader = serviceScope.ServiceProvider.GetRequiredService<IAssetsLoaderService>();
            var genreService = serviceScope.ServiceProvider.GetRequiredService<IGenreService>();
            var genres = await assetsLoader.LoadJsonAsset<string[]>("DefaultGenres");
    
            await genreService.EnsureAllExist(genres);

            var movies = await assetsLoader.LoadJsonAsset<string[][]>("DemoMovies");
            var movieService = serviceScope.ServiceProvider.GetRequiredService<IMovieService>();

            foreach (var movieArr in movies)
            {
                var name = movieArr[0];
                var movieGenres = movieArr.Skip(1).ToList();

                if (await movieService.FindMovieByName(name) == null)
                {
                    await genreService.EnsureAllExist(movieGenres);
                    await movieService.CreateMovie(new CreateNewMovieDto()
                    {
                        Name = name,
                        GenreIds = (await genreService.FindGenres(movieGenres)).Select(g => g.Id).ToList(),
                    });
                }
            }
        }
    }
}