using System.Runtime.Serialization;

namespace AEXMovies.Services.MovieService.Exceptions;

[Serializable]
public class MovieException : Exception
{
    public MovieException()
    {
    }

    public MovieException(string message) : base(message)
    {
    }

    public MovieException(string message, Exception inner) : base(message, inner)
    {
    }

    protected MovieException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}