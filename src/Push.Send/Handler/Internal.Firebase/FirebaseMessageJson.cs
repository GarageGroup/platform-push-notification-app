using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GarageGroup.Platform.PushNotification;

internal sealed record class FirebaseMessageJson
{
    public FirebaseMessageJson(string token, FirebaseNotificationJson notification)
    {
        Token = token.OrEmpty();
        Notification = notification;
    }

    public string Token { get; }

    public FirebaseNotificationJson Notification { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyDictionary<string, string>? Data { get; init; }
}