using AEXMovies.Repositories.ActorRepository;
using AEXMovies.Repositories.GenreRepository;
using AEXMovies.Services.ActorService;
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
            var actorService = serviceScope.ServiceProvider.GetRequiredService<IActorService>();

            var genres = await assetsLoader.LoadJsonAsset<string[]>("DefaultGenres");

            await genreService.EnsureAllExist(genres);

            var actors = await assetsLoader.LoadJsonAsset<string[]>("Actors");
            foreach (var actor in actors)
                await actorService.Create(new CreateActorDto
                {
                    Name = actor
                });


            var movies = await assetsLoader.LoadJsonAsset<string[][]>("DemoMovies");
            var movieService = serviceScope.ServiceProvider.GetRequiredService<IMovieService>();

            var genreRepository = serviceScope.ServiceProvider.GetRequiredService<IGenreRepository>();
            var actorRepository = serviceScope.ServiceProvider.GetRequiredService<IActorRepository>();
            var allActors = await actorRepository.FindMany(a => actors.Contains(a.Name));

            foreach (var movieArr in movies)
            {
                var name = movieArr[0];
                var movieGenres = movieArr.Skip(1).ToList();

                if (await movieService.FindMovieByName(name) == null)
                {
                    await genreService.EnsureAllExist(movieGenres);


                    await movieService.CreateMovie(new CreateNewMovieDto
                    {
                        Name = name,
                        // note: we use ms sql and it's case insensitive, so it's fine to compare NormalizedName to whatever is in movieGenres
                        GenreIds = (await genreRepository.FindMany(g => movieGenres.Contains(g.NormalizedName)))
                            .Select(g => g.Id).ToList(),
                        ActorIds = allActors.Select(a => a.Id).ToList()
                    });
                }
            }
        }
    }
}