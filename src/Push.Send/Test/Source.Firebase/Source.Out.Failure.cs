using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace GarageGroup.Platform.PushNotification.Push.Send.Test;

partial class FirebaseSendFuncTestSource
{
    public static TheoryData<HttpResponseMessage, Failure<FirebaseSendFailureCode>> OutputFailureTestData
        =>
        new()
        {
            {
                new()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                },
                new(FirebaseSendFailureCode.InvalidArgument, "An unexpected Firebase error occured")
            },
            {
                new()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("You have no permission for this action"),
                },
                new(FirebaseSendFailureCode.InvalidArgument, "You have no permission for this action")
            },
            {
                new()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent(string.Empty),
                },
                new(FirebaseSendFailureCode.Unregistered, "An unexpected Firebase error occured")
            },
            {
                new()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Not found text"),
                },
                new(FirebaseSendFailureCode.Unregistered, "Not found text")
            },
            {
                new()
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    Content = new StringContent("Request forbidden text"),
                },
                new(FirebaseSendFailureCode.SenderIdMismatch, "Request forbidden text")
            },
            {
                new()
                {
                    StatusCode = HttpStatusCode.TooManyRequests,
                    Content = new StringContent("TooManyRequests text"),
                },
                new(FirebaseSendFailureCode.QuotaExceeded, "TooManyRequests text")
            },
            {
                new()
                {
                    StatusCode = HttpStatusCode.ServiceUnavailable,
                    Content = new StringContent("ServiceUnavailable text"),
                },
                new(FirebaseSendFailureCode.Unavailable, "ServiceUnavailable text")
            },
            {
                new()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("InternalServerError text"),
                },
                new(FirebaseSendFailureCode.Internal, "InternalServerError text")
            },
            {
                new()
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent("Unauthorized text"),
                },
                new(FirebaseSendFailureCode.AuthorizationError, "Unauthorized text")
            },
        };
}