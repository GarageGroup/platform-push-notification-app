using GarageGroup.Infra;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeFuncPack;

namespace GarageGroup.Platform.PushNotification;

partial class ApplicationService
{
    internal static void Configure(IFunctionsWorkerApplicationBuilder builder)
        =>
        builder.Services.RegisterTokenErrorBusApiIfConfigured().RegisterPushTokenBusApi();

    private static IServiceCollection RegisterTokenErrorBusApiIfConfigured(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        var pushTokenErrorQueueName = configuration[BusPushTokenErrorQueueKey];
        if (string.IsNullOrWhiteSpace(pushTokenErrorQueueName))
        {
            return services;
        }

        return services.RegisterTokenErrorBusApi();
    }

    private static IServiceCollection RegisterTokenErrorBusApi(this IServiceCollection services)
        =>
        ServiceBusMessageApi.Configure<PushTokenErrorJson>(
            BusConnectionStringKey, BusPushTokenErrorQueueKey)
        .ToRegistrar(services)
        .RegisterSingleton();

    private static IServiceCollection RegisterPushTokenBusApi(this IServiceCollection services)
        =>
        ServiceBusMessageApi.Configure<PushSendIn>(
            BusConnectionStringKey, BusPushNotificationQueueKey)
        .ToRegistrar(services)
        .RegisterSingleton();
}