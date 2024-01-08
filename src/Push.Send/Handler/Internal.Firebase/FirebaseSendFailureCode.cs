namespace GarageGroup.Platform.PushNotification;

internal enum FirebaseSendFailureCode
{
    Unknown,

    InvalidArgument,

    Unregistered,

    SenderIdMismatch,

    QuotaExceeded,

    Unavailable,

    Internal,

    AuthorizationError
}