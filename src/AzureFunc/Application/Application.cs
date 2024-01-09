using System;
using Microsoft.Extensions.Configuration;

namespace GarageGroup.Platform.PushNotification;

internal static partial class Application
{
    private static T GetRequiredValue<T>(this IConfiguration configuration, string key)
        where T : class
        =>
        configuration.GetSection(key).Get<T>() ?? throw new InvalidOperationException($"Configuration section '{key}' must be specified");
}