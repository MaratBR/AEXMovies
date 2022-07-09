using System.ComponentModel.DataAnnotations;

namespace AEXMovies.Util;

public static class ConfigurationExtensions
{
    public static T GetAndValidateRequiredSection<T>(this IConfiguration configuration)
    {
        var type = typeof(T);
        var value = configuration.GetRequiredSection(type.Name).Get<T>();
        if (value == null)
            throw new ValidationException($"Configuration section {type.Name} could not be converted to type {type}");
        ;

        try
        {
            Validator.ValidateObject(value, new ValidationContext(value), true);
        }
        catch (ValidationException e)
        {
            throw new ValidationException($"Failed to validate config section {type.Name}: {e.Message}", e);
        }

        return value;
    }
}