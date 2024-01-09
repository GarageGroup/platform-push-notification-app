using GarageGroup.Infra;
using Microsoft.Extensions.DependencyInjection;
using PrimeFuncPack;

namespace GarageGroup.Platform.PushNotification;

partial class Application
{
    [HttpFunction("HealthCheck", HttpMethodName.Get, Route = "health", AuthLevel = HttpAuthorizationLevel.Function)]
    internal static Dependency<IHealthCheckHandler> UseHealthCheck()
        =>
        Dependency.From(
            ServiceProviderServiceExtensions.GetRequiredService<IBusMessageApi<PushSendIn>>)
        .UseServiceHealthCheckApi("PushNotificationBusApi")
        .UseHealthCheckHandler();
}