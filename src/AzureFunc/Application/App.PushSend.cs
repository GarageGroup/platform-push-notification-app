using System;
using GarageGroup.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeFuncPack;

namespace GarageGroup.Platform.PushNotification;

partial class Application
{
    [ServiceBusFunction("SendPushNotification", $"%{ApplicationService.BusPushTokenQueueKey}%", ApplicationService.BusConnectionStringKey)]
    internal static Dependency<IPushSendHandler> UsePushSendHandler()
        =>
        Pipeline.Pipe(
            PrimaryHandler.UseStandardSocketsHttpHandler().UseLogging("PushSend").UsePollyStandard())
        .With(
            ResolvePushSendOption,
            ServiceProviderServiceExtensions.GetService<IBusMessageApi<PushTokenErrorJson>>)
        .UsePushSendHandler();

    private static PushSendOption ResolvePushSendOption(IServiceProvider serviceProvider)
    {
        var section = serviceProvider.GetRequiredService<IConfiguration>().GetRequiredSection("Google");
        var googleCredentialJson = section["CredentialJson"];

        if (string.IsNullOrWhiteSpace(googleCredentialJson))
        {
            throw new InvalidOperationException("CredentialJson must be specified");
        }

        var serviceUri = section.GetValue<Uri>("ServiceUri") ?? throw new InvalidOperationException("ServiceUri must be specified");

        var projectName = section["ProjectName"];
        if (string.IsNullOrWhiteSpace(projectName))
        {
            throw new InvalidOperationException("ProjectName must be specified");
        }

        return new(
            serviceUri: serviceUri,
            projectName: projectName,
            googleCredentialJson: googleCredentialJson);
    }
}