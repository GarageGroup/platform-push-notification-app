using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Platform.PushNotification;

internal readonly record struct FirebaseNotificationJson
{
    public FirebaseNotificationJson([AllowNull] string title, [AllowNull] string body)
    {
        Title = title.OrNullIfEmpty();
        Body = body.OrNullIfEmpty();
    }

    public string? Title { get; }

    public string? Body { get; }
};