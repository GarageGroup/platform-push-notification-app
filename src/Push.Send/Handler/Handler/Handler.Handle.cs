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
        .ForwardValue(
            InnerHandlerAsync);

    private ValueTask<Result<Unit, Failure<HandlerFailureCode>>> InnerHandlerAsync(
        PushSendIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            static @in => new FirebaseSendIn(
                message: new()
                {
                    Token = @in.Token,
                    Notification = new(
                        body: @in.Notification?.Body,
                        title: @in.Notification?.Title),
                    Data = @in.Data
                }))
        .PipeValue(
            firebaseSendFunc.InvokeAsync)
        .MapFailure(
            static failure => failure.MapFailureCode(MapFailureCode))
        .OnFailureValue(
            (failure, token) => ProcessFailureAsync(input, failure, token));

    private async ValueTask<Unit> ProcessFailureAsync(
        PushSendIn? input, Failure<HandlerFailureCode> failure, CancellationToken cancellationToken)
    {
        if (tokenErrorBusApi is null || failure.FailureCode is not HandlerFailureCode.Persistent)
        {
            return default;
        }

        var busMessageInput = new BusMessageSendIn<PushTokenErrorJson>(
            message: new()
            {
                PushToken = input?.Token
            });

        return await tokenErrorBusApi.SendMessageAsync(busMessageInput, cancellationToken).ConfigureAwait(false);
    }
}