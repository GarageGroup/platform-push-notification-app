using System;
using System.Collections.Generic;
using System.Threading;
using GarageGroup.Infra;
using Moq;

namespace GarageGroup.Platform.PushNotification.Push.Send.Test;

public static partial class PushSendHandlerTest
{
    private static readonly PushSendIn SomeInput
        =
        new(
            pushToken: "3c575a1c42274f90a4d3a5f838522f7c",
            title: "New Opportunities (5)",
            body: "We have some new properties to offer, open mobile app to offer it",
            data: new Dictionary<string, string>
            {
                ["FirstKey"] = "Some first value",
                ["SecondKey"] = "Some second value"
            });

    private static Mock<IFirebaseSendFunc> BuildMockFirebaseSendFunc(in Result<Unit, Failure<FirebaseSendFailureCode>> result)
    {
        var mock = new Mock<IFirebaseSendFunc>();

        _ = mock.Setup(
            static f => f.InvokeAsync(It.IsAny<FirebaseSendIn>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(result);

        return mock;
    }

    private static Mock<IBusMessageSendSupplier<PushTokenErrorJson>> CreateMockTokenErrorBusApi()
    {
        var mock = new Mock<IBusMessageSendSupplier<PushTokenErrorJson>>();

        _ = mock.Setup(
            static a => a.SendMessageAsync(It.IsAny<BusMessageSendIn<PushTokenErrorJson>>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(Unit.Value);

        return mock;
    }
}