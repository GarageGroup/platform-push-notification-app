using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using GarageGroup.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GarageGroup.Platform.PushNotification.Push.Send.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace GarageGroup.Platform.PushNotification;

public static class PushSendDependency
{
    public static Dependency<IPushSendHandler> UsePushSendHandler<TTokenErrorBusApi>(
        this Dependency<HttpMessageHandler, PushSendOption, TTokenErrorBusApi?> dependency)
        where TTokenErrorBusApi : IBusMessageSendSupplier<PushTokenErrorJson>
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<IPushSendHandler>(CreateHandler);

        static PushSendHandler CreateHandler(HttpMessageHandler httpMessageHandler, PushSendOption option, TTokenErrorBusApi? tokenErrorBusApi)
        {
            ArgumentNullException.ThrowIfNull(httpMessageHandler);
            ArgumentNullException.ThrowIfNull(option);

            return new(
                firebaseSendFunc: new ImplFirebaseSendFunc(
                    httpMessageHandler: new FirebaseAuthorizationHandler(option, httpMessageHandler),
                    option: option),
                tokenErrorBusApi: tokenErrorBusApi);
        }
    }
}