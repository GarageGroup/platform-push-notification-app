using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GarageGroup.Platform.PushNotification;

internal sealed record class FirebaseMessageJson
{
    public string? Token { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FirebaseNotificationJson Notification { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyDictionary<string, string>? Data { get; init; }
}