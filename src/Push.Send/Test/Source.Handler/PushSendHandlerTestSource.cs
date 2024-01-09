using System.Collections.Generic;

namespace GarageGroup.Platform.PushNotification.Push.Send.Test;

internal static partial class PushSendHandlerTestSource
{
    private static readonly IReadOnlyDictionary<string, string> SomeData
        =
        new Dictionary<string, string>
        {
            ["Frst key"] = "Some text",
            [string.Empty] = "Some value",
            ["SomeKey"] = string.Empty
        };
}