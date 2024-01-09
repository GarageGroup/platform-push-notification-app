namespace GarageGroup.Platform.PushNotification;

public sealed record class PushSendOption
{
    public PushSendOption(GoogleCredentialJson credential)
        =>
        Credential = credential ?? new();

    public GoogleCredentialJson Credential { get; }
}