using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Platform.PushNotification;

internal sealed class ImplFirebaseSendFunc(HttpMessageHandler httpMessageHandler, string projectId) : IFirebaseSendFunc
{
    private static readonly Uri ServiceUri;

    private static readonly JsonSerializerOptions SerializerOptions;

    static ImplFirebaseSendFunc()
    {
        ServiceUri = new("https://fcm.googleapis.com");
        SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async ValueTask<Result<Unit, Failure<FirebaseSendFailureCode>>> InvokeAsync(
        FirebaseSendIn input, CancellationToken cancellationToken)
    {
        var httpClient = new HttpClient(httpMessageHandler, disposeHandler: false)
        {
            BaseAddress = ServiceUri
        };

        var json = new FirebaseBodyJson(input.Message);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"/v1/projects/{projectId}/messages:send")
        {
            Content = JsonContent.Create(json, null, SerializerOptions),
        };

        using var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        if (httpResponse.IsSuccessStatusCode)
        {
            return Result.Success<Unit>(default);
        }

        var failureContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        return Failure.Create(
            failureCode: httpResponse.StatusCode switch
            {
                HttpStatusCode.BadRequest => FirebaseSendFailureCode.InvalidArgument,
                HttpStatusCode.NotFound => FirebaseSendFailureCode.Unregistered,
                HttpStatusCode.Forbidden => FirebaseSendFailureCode.SenderIdMismatch,
                HttpStatusCode.TooManyRequests => FirebaseSendFailureCode.QuotaExceeded,
                HttpStatusCode.ServiceUnavailable => FirebaseSendFailureCode.Unavailable,
                HttpStatusCode.InternalServerError => FirebaseSendFailureCode.Internal,
                HttpStatusCode.Unauthorized => FirebaseSendFailureCode.AuthorizationError,
                _ => FirebaseSendFailureCode.Unknown,
            },
            failureMessage: failureContent.OrNullIfEmpty() ?? "An unexpected Firebase error occured");
    }

    private sealed record class FirebaseBodyJson(FirebaseMessageJson Message);
}