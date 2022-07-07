namespace AEXMovies.Repositories.Exceptions;

[Serializable]
public class ModelNotFound : Exception
{
    public ModelNotFound(string message) : base(message)
    {
    }
}