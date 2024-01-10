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
                new(HandlerFailureCode.Persistent, "Token must be specified")
            },
            {
                new(
                    token: null!,
                    notification: default,
                    data: new Dictionary<string, string>
                    {
                        ["Some key"] = "Some value"
                    }),
                new(HandlerFailureCode.Persistent, "Token must be specified")
            },
            {
                new(
                    token: " \n \n",
                    notification: new()
                    {
                        Title = "Some title",
                        Body = "Some body"
                    },
                    data: default),
                new(HandlerFailureCode.Persistent, "Token must be specified")
            }
        };
}