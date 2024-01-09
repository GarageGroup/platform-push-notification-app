using System.Collections.Generic;
using Xunit;

namespace GarageGroup.Platform.PushNotification.Push.Send.Test;

partial class PushSendHandlerTestSource
{
    public static TheoryData<PushSendIn, FirebaseSendIn> InputValidTestData
        =>
        new()
        {
            {
                new(
                    pushToken: "emS19F14",
                    title: "Test title",
                    body: "This notification was sent with test purpose. Just ignore it.",
                    data: null),
                new FirebaseSendIn(
                    message: new(
                        token: "emS19F14",
                        notification: new(
                            title: "Test title",
                            body: "This notification was sent with test purpose. Just ignore it."))
                    {
                        Data = null
                    })
            },
            {
                new(
                    pushToken: "SomePushToken",
                    title: "Some title",
                    body: "Some body",
                    data: new Dictionary<string, string>(5)),
                new(
                    message: new(
                        token: "SomePushToken",
                        notification: new(
                            title: "Some title",
                            body: "Some body"))
                    {
                        Data = null
                    })
            },
            {
                new(
                    pushToken: "Some token",
                    title: "Some Title",
                    body: "SomeBody",
                    data: SomeData),
                new(
                    message: new(
                        token: "Some token",
                        notification: new(
                            title: "Some Title",
                            body: "SomeBody"))
                    {
                        Data = SomeData
                    })
            }
        };
}