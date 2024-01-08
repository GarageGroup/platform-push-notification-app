using System;
using System.Collections.Generic;
using Xunit;

namespace GarageGroup.Platform.PushNotification.Push.Send.Test;

partial class FirebaseSendFuncTestSource
{
    public static TheoryData<PushSendOption, FirebaseSendIn, Uri, string> InputValidTestData
        =>
        new()
        {
            {
                new(
                    googleCredentialJson: "testKey",
                    serviceUri: new("https://testFcmUri.com"),
                    projectName: "SomeProject"),
                new(
                    message: new(
                        token: "someToken",
                        notification: new("test title", "test body"))
                    {
                        Data = null
                    }),
                new("https://testFcmUri.com/v1/projects/SomeProject/messages:send"),
                InnerBuildFirebaseJson(
                    message: new(
                        token: "someToken",
                        notification: new("test title", "test body"))
                    {
                        Data = null
                    })
            },
            {
                new(
                    googleCredentialJson: "SomeKey",
                    serviceUri: new("http://some-uri.site/"),
                    projectName: "project/test//"),
                new(
                    message: new(
                        token: "SomeToken",
                        notification: new("Some title", "Some body"))
                    {
                        Data = new Dictionary<string, string>()
                    }),
                new("http://some-uri.site/v1/projects/project/test///messages:send"),
                InnerBuildFirebaseJson(
                    message: new(
                        token: "SomeToken",
                        notification: new("Some title", "Some body"))
                    {
                        Data = new Dictionary<string, string>()
                    })
            },
            {
                new(
                    googleCredentialJson: "Some Key",
                    serviceUri: new("http://some-uri.site/"),
                    projectName: "Some Project"),
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
                new("http://some-uri.site/v1/projects/Some Project/messages:send"),
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