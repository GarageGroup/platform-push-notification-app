using System;

namespace GarageGroup.Platform.PushNotification;

internal sealed record class FirebaseNotificationJson
{
    public FirebaseNotificationJson(string title, string body)
    {
        Title = title.OrEmpty();
        Body = body.OrEmpty();
    }

    public string Title { get; }

    public string Body { get; }
};