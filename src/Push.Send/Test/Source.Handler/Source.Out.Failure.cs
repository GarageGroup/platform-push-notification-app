using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Platform.PushNotification.Push.Send.Test;

partial class PushSendHandlerTestSource
{
    public static TheoryData<IBusMessageSendSupplier<PushTokenErrorJson>?, FirebaseSendFailureCode, HandlerFailureCode> OutputFailureTestData
    {
        get
        {
            var data = new TheoryData<IBusMessageSendSupplier<PushTokenErrorJson>?, FirebaseSendFailureCode, HandlerFailureCode>();

            var persistentFailureCodes = new[]
            {
                FirebaseSendFailureCode.InvalidArgument,
                FirebaseSendFailureCode.Unregistered
            };

            var tokenErrorBusApi = new MockTokenErrorBusApi();

            foreach (var persistentFailureCode in persistentFailureCodes)
            {
                data.Add(null, persistentFailureCode, HandlerFailureCode.Persistent);
                data.Add(tokenErrorBusApi, persistentFailureCode, HandlerFailureCode.Persistent);
            }

            foreach (var transientFailureCode in Enum.GetValues<FirebaseSendFailureCode>().Where(IsNotPersistentFailureCode))
            {
                data.Add(null, transientFailureCode, HandlerFailureCode.Transient);
                data.Add(tokenErrorBusApi, transientFailureCode, HandlerFailureCode.Transient);
            }

            return data;

            bool IsNotPersistentFailureCode(FirebaseSendFailureCode failureCode)
                =>
                persistentFailureCodes.Contains(failureCode) is false;
        }
    }
}

file sealed class MockTokenErrorBusApi : IBusMessageSendSupplier<PushTokenErrorJson>
{
    public Task<Unit> SendMessageAsync(
        BusMessageSendIn<PushTokenErrorJson> input, CancellationToken cancellationToken)
        =>
        Task.FromResult<Unit>(default);

    public Task<Result<Unit, Failure<Unit>>> SendMessageOrFailureAsync(
        BusMessageSendIn<PushTokenErrorJson> input, CancellationToken cancellationToken)
        =>
        Task.FromResult<Result<Unit, Failure<Unit>>>(Result.Success<Unit>(default));
}