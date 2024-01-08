namespace GarageGroup.Platform.PushNotification;

public sealed record class PushTokenErrorJson
{
    public string? PushToken { get; init; }
}