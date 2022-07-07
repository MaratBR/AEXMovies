using AEXMovies.Models;
using Microsoft.EntityFrameworkCore;

namespace AEXMovies.Repositories.RefreshTokenRepository;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly EfDbContext _context;
    
    public RefreshTokenRepository(EfDbContext context)
    {
        _context = context;
    }


    public async Task Insert(RefreshToken token)
    {
        if (_context.Entry(token).State != EntityState.Detached)
            throw new InvalidOperationException("Cannot insert refresh token that is already being tracked by the context");
        _context.RefreshTokens.Add(token);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(RefreshToken token)
    {
        if (_context.Entry(token).State == EntityState.Deleted) return;
        _context.RefreshTokens.Remove(token);
        await _context.SaveChangesAsync();
    }

    public Task<RefreshToken?> Get(string id)
    {
        return _context.RefreshTokens.Where(t => t.Id == id).SingleOrDefaultAsync();
    }
}