namespace AEXMovies.Repositories;

public interface IHasId<out T>
{
    public T Id { get; }
}