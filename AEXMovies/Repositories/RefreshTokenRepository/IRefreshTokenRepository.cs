using AEXMovies.Models;

namespace AEXMovies.Repositories.RefreshTokenRepository;

public interface IRefreshTokenRepository
{
    Task Insert(RefreshToken token);
    Task Delete(RefreshToken token);
    Task<RefreshToken?> Get(string id);
}