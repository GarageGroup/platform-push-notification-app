using System;
using System.Collections.Generic;
using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Platform.PushNotification.Push.Send.Test;

partial class PushSendHandlerTestSource
{
    public static TheoryData<PushSendIn, Failure<HandlerFailureCode>> InputInvalidPushTokenTestData
        =>
        new()
        {
            {
                new(
                    pushToken: null!,
                    title: "some title",
                    body: "some body",
                    data: new Dictionary<string, string>
                    {
                        ["SomeKey"] = "Some value"
                    }),
                Failure.Create(HandlerFailureCode.Persistent, "PushToken must be specified")
            },
            {
                new(
                    pushToken: " \n \n",
                    title: "some title",
                    body: "some body",
                    data: new Dictionary<string, string>
                    {
                        ["Some key"] = "Some value"
                    }),
                Failure.Create(HandlerFailureCode.Persistent, "PushToken must be specified")
            }
        };
}