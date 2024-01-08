using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;

namespace GarageGroup.Platform.PushNotification;

internal sealed class FirebaseAuthorizationHandler(PushSendOption option, HttpMessageHandler innerHandler) : DelegatingHandler(innerHandler)
{
    private const string MessagingScope = "https://www.googleapis.com/auth/firebase.messaging";

    private const string CloudPlatformScope = "https://www.googleapis.com/auth/cloud-platform";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await GetAccessTokenAsync(option.GoogleCredentialJson, cancellationToken).ConfigureAwait(false);
        request.Headers.Authorization = new("Bearer", accessToken);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private static Task<string> GetAccessTokenAsync(string serviceAccountPath, CancellationToken cancellationToken)
    {
        var credential = GoogleCredential.FromJson(serviceAccountPath).CreateScoped(MessagingScope, CloudPlatformScope);
        return credential.UnderlyingCredential.GetAccessTokenForRequestAsync(cancellationToken: cancellationToken);
    }
}