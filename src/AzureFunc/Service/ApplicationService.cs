namespace GarageGroup.Platform.PushNotification;

internal static partial class ApplicationService
{
    public const string BusConnectionStringKey = "ServiceBus:ConnectionString";

    public const string BusPushTokenQueueKey = "ServiceBus:PushTokenQueue";

    private const string BusPushTokenErrorQueueKey = "ServiceBus:PushTokenErrorQueue";
}