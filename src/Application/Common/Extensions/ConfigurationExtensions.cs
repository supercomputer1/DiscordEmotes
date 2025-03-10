using Microsoft.Extensions.Configuration;

namespace Application.Common.Extensions;

public static class ConfigurationExtensions
{
    public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
    {
        return configuration.GetValue<T>(key)
            ?? throw new Exception($"Could not find a configuration value for key {key}.");
    }
}
