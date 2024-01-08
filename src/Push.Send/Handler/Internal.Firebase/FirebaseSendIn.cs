namespace GarageGroup.Platform.PushNotification;

internal sealed record class FirebaseSendIn
{
    public FirebaseSendIn(FirebaseMessageJson message)
        =>
        Message = message;

    public FirebaseMessageJson Message { get; }
}