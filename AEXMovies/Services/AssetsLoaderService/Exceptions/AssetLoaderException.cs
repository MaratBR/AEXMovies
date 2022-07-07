namespace AEXMovies.Services.AssetsLoaderService.Exceptions;

[Serializable]
public class AssetLoaderException : Exception
{
    public AssetLoaderException(string message) : base(message)
    {
    }

    public AssetLoaderException(string message, Exception exception) : base(message, exception)
    {
    }
}