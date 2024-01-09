using System;
using GarageGroup.Infra;

namespace GarageGroup.Platform.PushNotification;

using ITokenErrorBusApi = IBusMessageSendSupplier<PushTokenErrorJson>;

internal sealed partial class PushSendHandler(IFirebaseSendFunc firebaseSendFunc, ITokenErrorBusApi? tokenErrorBusApi) : IPushSendHandler
{
    private static Result<PushSendIn, Failure<HandlerFailureCode>> ValidateInput(PushSendIn? input)
    {
        if (string.IsNullOrWhiteSpace(input?.Token))
        {
            return Failure.Create(HandlerFailureCode.Persistent, "Token must be specified");
        }

        return Result.Success(input);
    }

    private static HandlerFailureCode MapFailureCode(FirebaseSendFailureCode failureCode)
        =>
        failureCode switch
        {
            FirebaseSendFailureCode.Unregistered or FirebaseSendFailureCode.InvalidArgument => HandlerFailureCode.Persistent,
            _ => HandlerFailureCode.Transient
        };
}