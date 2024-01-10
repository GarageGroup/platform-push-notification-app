using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Platform.PushNotification;

public sealed record class PushSendIn
{
    public PushSendIn(string token, PushNotification? notification, [AllowNull] IReadOnlyDictionary<string, string> data)
    {
        Token = token.OrEmpty();
        Notification = notification;
        Data = data?.Count is not > 0 ? null : data;
    }

    public string Token { get; }

    public PushNotification? Notification { get; }

    public IReadOnlyDictionary<string, string>? Data { get; }
}