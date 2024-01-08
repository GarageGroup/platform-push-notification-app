using System;

namespace GarageGroup.Platform.PushNotification;

public sealed record class PushSendOption
{
    public PushSendOption(Uri serviceUri, string projectName, string googleCredentialJson)
    {
        ServiceUri = serviceUri;
        ProjectName = projectName.OrEmpty();
        GoogleCredentialJson = googleCredentialJson.OrEmpty();
    }

    public Uri ServiceUri { get; }

    public string ProjectName { get; }

    public string GoogleCredentialJson { get; }
}