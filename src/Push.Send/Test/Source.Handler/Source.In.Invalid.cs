using System;
using System.Collections.Generic;
using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Platform.PushNotification.Push.Send.Test;

partial class PushSendHandlerTestSource
{
    public static TheoryData<PushSendIn?, Failure<HandlerFailureCode>> InputInvalidTestData
        =>
        new()
        {
            {
                null,
                new(HandlerFailureCode.Persistent, "Input must be specified")
            },
            {
                new(
                    pushToken: "some token",
                    title: null!,
                    body: "some body",
                    data: new Dictionary<string, string>
                    {
                        ["Some key"] = "Some value"
                    }),
                new(HandlerFailureCode.Persistent, "Title must be specified")
            },
            {
                new(
                    pushToken: "some token",
                    title: "\n\n   ",
                    body: "some body",
                    data: new Dictionary<string, string>
                    {
                        ["Some key"] = "Some value"
                    }),
                new(HandlerFailureCode.Persistent, "Title must be specified")
            },
            {
                new(
                    pushToken: "some token",
                    title: "some title",
                    body: null!,
                    data: new Dictionary<string, string>
                    {
                        ["Some key"] = "Some value"
                    }),
                new(HandlerFailureCode.Persistent, "Body must be specified")
            },
            {
                new(
                    pushToken: "some token",
                    title: "some title",
                    body: "\n \n \n ",
                    data: new Dictionary<string, string>
                    {
                        ["Some key"] = "Some value"
                    }),
                new(HandlerFailureCode.Persistent, "Body must be specified")
            },
        };
}