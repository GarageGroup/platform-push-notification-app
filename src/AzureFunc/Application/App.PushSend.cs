using System;
using GarageGroup.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeFuncPack;

namespace GarageGroup.Platform.PushNotification;

partial class Application
{
    [ServiceBusFunction("SendPushNotification", $"%{ApplicationService.BusPushNotificationQueueKey}%", ApplicationService.BusConnectionStringKey)]
    internal static Dependency<IPushSendHandler> UsePushSendHandler()
        =>
        Pipeline.Pipe(
            PrimaryHandler.UseStandardSocketsHttpHandler().UseLogging("PushSend").UsePollyStandard())
        .With(
            ResolvePushSendOption,
            ServiceProviderServiceExtensions.GetService<IBusMessageApi<PushTokenErrorJson>>)
        .UsePushSendHandler();

    private static PushSendOption ResolvePushSendOption(IServiceProvider serviceProvider)
        =>
        new(
            credential: serviceProvider.GetRequiredService<IConfiguration>().GetRequiredValue<GoogleCredentialJson>("Google:Credential"));
}