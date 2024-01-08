using GarageGroup.Infra;

namespace GarageGroup.Platform.PushNotification;

using ITokenErrorBusApi = IBusMessageSendSupplier<PushTokenErrorJson>;

internal sealed partial class PushSendHandler(IFirebaseSendFunc firebaseSendFunc, ITokenErrorBusApi? tokenErrorBusApi) : IPushSendHandler
{
    private enum PushSendFailureCode
    {
        Unknown,

        InvalidInput,

        InvalidPushToken
    }

    private static PushSendFailureCode MapFirebaseSendFailureCode(FirebaseSendFailureCode failureCode)
        =>
        failureCode switch
        {
            FirebaseSendFailureCode.Unregistered or FirebaseSendFailureCode.InvalidArgument => PushSendFailureCode.InvalidPushToken,
            _ => PushSendFailureCode.Unknown
        };

    private static HandlerFailureCode MapFailureCode(PushSendFailureCode failureCode)
        =>
        failureCode switch
        {
            PushSendFailureCode.InvalidPushToken or PushSendFailureCode.InvalidInput => HandlerFailureCode.Persistent,
            _ => HandlerFailureCode.Transient
        };
}