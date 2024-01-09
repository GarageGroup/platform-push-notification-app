using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Platform.PushNotification;

partial class PushSendHandler
{
    public ValueTask<Result<Unit, Failure<HandlerFailureCode>>> HandleAsync(
        PushSendIn? input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            ValidateInput)
        .MapSuccess(
            static @in => new FirebaseSendIn(
                message: new(
                    token: @in.PushToken,
                    notification: new(
                        body: @in.Body,
                        title: @in.Title))
                {
                    Data = @in.Data
                }))
        .ForwardValue(
            firebaseSendFunc.InvokeAsync,
            static failure => failure.MapFailureCode(MapFirebaseSendFailureCode))
        .OnFailureValue(
            (failure, token) => ProcessFailureAsync(input, failure, token))
        .MapFailure(
            static failure => failure.MapFailureCode(MapFailureCode));

    private static Result<PushSendIn, Failure<PushSendFailureCode>> ValidateInput(PushSendIn? input)
    {
        if (input is null)
        {
            return Failure.Create(PushSendFailureCode.InvalidInput, "Input must be specified");
        }

        if (string.IsNullOrWhiteSpace(input.PushToken))
        {
            return Failure.Create(PushSendFailureCode.InvalidPushToken, "PushToken must be specified");
        }

        if (string.IsNullOrWhiteSpace(input.Title))
        {
            return Failure.Create(PushSendFailureCode.InvalidInput, "Title must be specified");
        }

        if (string.IsNullOrWhiteSpace(input.Body))
        {
            return Failure.Create(PushSendFailureCode.InvalidInput, "Body must be specified");
        }

        return input;
    }

    private async ValueTask<Unit> ProcessFailureAsync(
        PushSendIn? input, Failure<PushSendFailureCode> failure, CancellationToken cancellationToken)
    {
        if (failure.FailureCode is not PushSendFailureCode.InvalidPushToken || tokenErrorBusApi is null)
        {
            return default;
        }

        var busMessageInput = new BusMessageSendIn<PushTokenErrorJson>(
            message: new()
            {
                PushToken = input?.PushToken
            });

        return await tokenErrorBusApi.SendMessageAsync(busMessageInput, cancellationToken).ConfigureAwait(false);
    }
}