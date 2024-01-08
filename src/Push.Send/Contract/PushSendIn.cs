using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Platform.PushNotification;

public sealed record class PushSendIn
{
    public PushSendIn(string pushToken, string title, string body, [AllowNull] IReadOnlyDictionary<string, string> data)
    {
        PushToken = pushToken.OrEmpty();
        Title = title.OrEmpty();
        Body = body.OrEmpty();
        Data = data?.Count is not > 0 ? null : data;
    }

    public string PushToken { get; }

    public string Title { get; }

    public string Body { get; }

    public IReadOnlyDictionary<string, string>? Data { get; }
}