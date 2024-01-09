using System.Net;
using System.Net.Http;
using Xunit;

namespace GarageGroup.Platform.PushNotification.Push.Send.Test;

partial class FirebaseSendFuncTestSource
{
    public static TheoryData<HttpResponseMessage> OutputSuccessTestData
        =>
        new()
        {
            {
                new()
                {
                    StatusCode = HttpStatusCode.OK
                }
            },
            {
                new()
                {
                    StatusCode = HttpStatusCode.NoContent
                }
            }
        };
}