namespace AEXMovies.Services.AuthService.Exceptions;

[Serializable]
public class UserException : Exception
{
    public UserException(string message) : base(message)
    {
        
    }
}