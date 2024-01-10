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
                    message: new()
                    {
                        Token = "someToken",
                        Notification = new("test title", "test body"),
                        Data = null
                    }),
                new("https://fcm.googleapis.com/v1/projects/SomeProject/messages:send"),
                InnerBuildFirebaseJson(
                    message: new()
                    {
                        Token = "someToken",
                        Notification = new("test title", "test body"),
                        Data = null
                    })
            },
            {
                "project/test//",
                new(
                    message: new()
                    {
                        Token = "SomeToken",
                        Notification = new(string.Empty, string.Empty),
                        Data = new Dictionary<string, string>()
                    }),
                new("https://fcm.googleapis.com/v1/projects/project/test///messages:send"),
                InnerBuildFirebaseJson(
                    message: new()
                    {
                        Token = "SomeToken",
                        Notification = default,
                        Data = new Dictionary<string, string>()
                    })
            },
            {
                "Some Project",
                new(
                    message: new()
                    {
                        Token = "Some token",
                        Notification = new(string.Empty, "Some body"),
                        Data = new Dictionary<string, string>
                        {
                            ["One"] = string.Empty,
                            ["Two"] = "Some text",
                            [string.Empty] = "Some Value"
                        }
                    }),
                new("https://fcm.googleapis.com/v1/projects/Some Project/messages:send"),
                InnerBuildFirebaseJson(
                    message: new FirebaseMessageJson()
                    {
                        Token = "Some token",
                        Notification = new(default, "Some body"),
                        Data = new Dictionary<string, string>
                        {
                            ["One"] = string.Empty,
                            ["Two"] = "Some text",
                            [string.Empty] = "Some Value"
                        }
                    })
            },
            {
                "Some Project Id",
                new(
                    message: new()
                    {
                        Token = "SomeToken",
                        Notification = new("Some Push Title", string.Empty),
                        Data = new Dictionary<string, string>
                        {
                            ["SomeKey"] = "SomeValue"
                        }
                    }),
                new("https://fcm.googleapis.com/v1/projects/Some Project Id/messages:send"),
                InnerBuildFirebaseJson(
                    message: new FirebaseMessageJson()
                    {
                        Token = "SomeToken",
                        Notification = new("Some Push Title", default),
                        Data = new Dictionary<string, string>
                        {
                            ["SomeKey"] = "SomeValue"
                        }
                    })
            }
        };
}