namespace AEXMovies.Services.AssetsLoaderService;

public interface IAssetsLoaderService
{
    Task<T> LoadJsonAsset<T>(string name);
}