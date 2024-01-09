using System.Collections.Generic;
using Xunit;

namespace GarageGroup.Platform.PushNotification.Push.Send.Test;

partial class PushSendHandlerTestSource
{
    public static TheoryData<PushSendIn, FirebaseSendIn> InputValidTestData
        =>
        new()
        {{
                new(
                    token: "SomeToken",
                    notification: null,
                    data: null),
                new(
                    message: new()
                    {
                        Token = "SomeToken",
                        Notification = default,
                        Data = null
                    })
            },
            {
                new(
                    token: "emS19F14",
                    notification: new()
                    {
                        Title = "Test title",
                        Body = "This notification was sent with test purpose. Just ignore it.",
                    },
                    data: null),
                new(
                    message: new()
                    {
                        Token = "emS19F14",
                        Notification = new(
                            title: "Test title",
                            body: "This notification was sent with test purpose. Just ignore it."),
                        Data = null
                    })
            },
            {
                new(
                    token: "SomePushToken",
                    notification: new()
                    {
                        Title = null,
                        Body = "Some body",
                    },
                    data: new Dictionary<string, string>(5)),
                new(
                    message: new()
                    {
                        Token = "SomePushToken",
                        Notification = new(
                            title: string.Empty,
                            body: "Some body"),
                        Data = null
                    })
            },
            {
                new(
                    token: "Some token",
                    notification: new()
                    {
                        Title = "Some Title",
                        Body = null,
                    },
                    data: SomeData),
                new(
                    message: new()
                    {
                        Token = "Some token",
                        Notification = new(
                            title: "Some Title",
                            body: string.Empty),
                        Data = SomeData
                    })
            }
        };
}