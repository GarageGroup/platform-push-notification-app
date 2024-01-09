using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Platform.PushNotification;

internal interface IFirebaseSendFunc
{
    ValueTask<Result<Unit, Failure<FirebaseSendFailureCode>>> InvokeAsync(
        FirebaseSendIn input, CancellationToken cancellationToken);
}
