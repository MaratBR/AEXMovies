using Microsoft.AspNetCore.Identity;

namespace AEXMovies.Services.AuthService.Exceptions;

public class UserIdentityErrorsException : UserException
{
    public UserIdentityErrorsException(List<IdentityError> errors) : base("Multiple errors occured")
    {
        Errors = errors;
    }

    public List<IdentityError> Errors { get; }
}