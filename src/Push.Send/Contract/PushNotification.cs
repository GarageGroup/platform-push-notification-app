namespace GarageGroup.Platform.PushNotification;

public sealed record class PushNotification
{
    public string? Title { get; init; }

    public string? Body { get; init; }
}