using Microsoft.AspNetCore.Identity;

namespace AEXMovies.Services.AuthService.Exceptions;

public class UserIdentityErrorsException : UserException
{
    public List<IdentityError> Errors { get; }

    public UserIdentityErrorsException(List<IdentityError> errors) : base("Multiple errors occured")
    {
        Errors = errors;
    }
}