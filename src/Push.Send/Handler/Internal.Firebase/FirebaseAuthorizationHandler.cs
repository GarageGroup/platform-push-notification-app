using System.Collections.Concurrent;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;

namespace GarageGroup.Platform.PushNotification;

internal sealed class FirebaseAuthorizationHandler(GoogleCredentialJson credential, HttpMessageHandler innerHandler) : DelegatingHandler(innerHandler)
{
    private const string MessagingScope = "https://www.googleapis.com/auth/firebase.messaging";

    private const string CloudPlatformScope = "https://www.googleapis.com/auth/cloud-platform";

    private static readonly JsonSerializerOptions SerializerOptions;

    private static readonly ConcurrentDictionary<GoogleCredentialJson, GoogleCredential> GoogleCredentials;

    static FirebaseAuthorizationHandler()
    {
        SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
        GoogleCredentials = new();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await GetAccessTokenAsync(credential, cancellationToken).ConfigureAwait(false);
        request.Headers.Authorization = new("Bearer", accessToken);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private static Task<string> GetAccessTokenAsync(GoogleCredentialJson credentialJson, CancellationToken cancellationToken)
    {
        var credential = GoogleCredentials.GetOrAdd(credentialJson, CreateGoogleCredential);
        return credential.UnderlyingCredential.GetAccessTokenForRequestAsync(cancellationToken: cancellationToken);
    }

    private static GoogleCredential CreateGoogleCredential(GoogleCredentialJson credentialJson)
    {
        var json = JsonSerializer.Serialize(credentialJson, SerializerOptions);
        return GoogleCredential.FromJson(json).CreateScoped(MessagingScope, CloudPlatformScope);
    }
}