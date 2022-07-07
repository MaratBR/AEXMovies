namespace AEXMovies.Services.AuthService.Exceptions;

[Serializable]
public class UserAlreadyExists : UserException
{
    public UserAlreadyExists(string userName) : base($"User \"{userName}\" already exists")
    {
    }
}