namespace GarageGroup.Platform.PushNotification.Push.Send.Test;

public static partial class FirebaseSendFuncTest
{
    private const string SomeProjectId
        =
        "SomeProjectId";

    private static readonly FirebaseSendIn SomeInput
        =
        new(
            message: new(
                token: "F1mc_dg1dFG",
                notification: new("Some messages are waiting for you...", "You have 6 unread messages")));
}