using System.Text.Json;

namespace GarageGroup.Platform.PushNotification.Push.Send.Test;

internal static partial class FirebaseSendFuncTestSource
{
    private static readonly JsonSerializerOptions SerializerOptions
        =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

    private static string InnerBuildFirebaseJson(FirebaseMessageJson message)
    {
        var bodyJson = new
        {
            Message = message
        };

        return JsonSerializer.Serialize(bodyJson, SerializerOptions);
    }
}