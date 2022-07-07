namespace AEXMovies.Models;

public class RefreshToken
{
    public string Id { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedById { get; set; } = string.Empty;
    public User CreatedBy { get; set; } = null!;
}