namespace GarageGroup.Platform.PushNotification;

public sealed record class GoogleCredentialJson
{
    public string? Type { get; init; }

    public string? ProjectId { get; init; }

    public string? PrivateKeyId { get; init; }

    public string? PrivateKey { get; init; }

    public string? ClientEmail { get; init; }

    public string? ClientId { get; init; }

    public string? AuthUri { get; init; }

    public string? TokenUri { get; init; }

    public string? AuthProviderX509CertUrl { get; init; }

    public string? ClientX509CertUrl { get; init; }
}