using System;
using System.Collections.Generic;
using Xunit;

namespace GarageGroup.Platform.PushNotification.Push.Send.Test;

partial class FirebaseSendFuncTestSource
{
    public static TheoryData<string, FirebaseSendIn, Uri, string> InputValidTestData
        =>
        new()
        {
            {
                "SomeProject",
                new(
                    message: new(
                        token: "someToken",
                        notification: new("test title", "test body"))
                    {
                        Data = null
                    }),
                new("https://fcm.googleapis.com/v1/projects/SomeProject/messages:send"),
                InnerBuildFirebaseJson(
                    message: new(
                        token: "someToken",
                        notification: new("test title", "test body"))
                    {
                        Data = null
                    })
            },
            {
                "project/test//",
                new(
                    message: new(
                        token: "SomeToken",
                        notification: new("Some title", "Some body"))
                    {
                        Data = new Dictionary<string, string>()
                    }),
                new("https://fcm.googleapis.com/v1/projects/project/test///messages:send"),
                InnerBuildFirebaseJson(
                    message: new(
                        token: "SomeToken",
                        notification: new("Some title", "Some body"))
                    {
                        Data = new Dictionary<string, string>()
                    })
            },
            {
                "Some Project",
                new(
                    message: new(
                        token: "Some token",
                        notification: new("Some Title", "Some body"))
                    {
                        Data = new Dictionary<string, string>
                        {
                            ["One"] = string.Empty,
                            ["Two"] = "Some text",
                            [string.Empty] = "Some Value"
                        }
                    }),
                new("https://fcm.googleapis.com/v1/projects/Some Project/messages:send"),
                InnerBuildFirebaseJson(
                    message: new FirebaseMessageJson(
                        token: "Some token",
                        notification: new("Some Title", "Some body"))
                    {
                        Data = new Dictionary<string, string>
                        {
                            ["One"] = string.Empty,
                            ["Two"] = "Some text",
                            [string.Empty] = "Some Value"
                        }
                    })
            }
        };
}