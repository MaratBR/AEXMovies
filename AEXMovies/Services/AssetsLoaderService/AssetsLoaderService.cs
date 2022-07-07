using System.Text.Json;
using AEXMovies.Services.AssetsLoaderService.Exceptions;
using Microsoft.Extensions.Options;

namespace AEXMovies.Services.AssetsLoaderService;

public class AssetsLoaderService : IAssetsLoaderService
{
    private readonly AssetLoaderOptions _options;

    public AssetsLoaderService(IOptions<AssetLoaderOptions> options)
    {
        _options = options.Value ?? new AssetLoaderOptions();
    }

    public async Task<T> LoadJsonAsset<T>(string name)
    {
        try
        {
            var content = await File.ReadAllTextAsync(Path.Join(_options.AssetsPath, name + ".json"));
            var value = JsonSerializer.Deserialize<T>(content);
            if (value == null)
                throw new AssetLoaderException($"JSON asset {name} is null");
            return value;
        }
        catch (Exception e)
        {
            throw new AssetLoaderException($"Failed to load asset {name}", e);
        }
    }
}