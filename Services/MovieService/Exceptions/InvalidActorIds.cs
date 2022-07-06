namespace AEXMovies.Services.MovieService.Exceptions;

[Serializable]
public class InvalidActorIds : MovieException
{
    public InvalidActorIds(IEnumerable<int> actorIds) : base($"Actors with following IDs could not be found: {string.Join(", ", actorIds)}")
    {
        
    }
}