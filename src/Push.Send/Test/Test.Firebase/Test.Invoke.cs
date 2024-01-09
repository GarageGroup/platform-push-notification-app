using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GarageGroup.Platform.PushNotification.Push.Send.Test;

partial class FirebaseSendFuncTest
{
    [Theory]
    [MemberData(nameof(FirebaseSendFuncTestSource.InputValidTestData), MemberType = typeof(FirebaseSendFuncTestSource))]
    internal static async Task InvokeAsync_ExpectHttpClientCalledOnce(
        string projectId, FirebaseSendIn input, Uri expectedRequestUri, string expectedContent)
    {
        using var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        using var mockMessageHandler = new MockHttpMessageHandler(responseMessage, OnHttpRequestAsync);

        var firebaseSendFunc = new ImplFirebaseSendFunc(mockMessageHandler, projectId);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await firebaseSendFunc.InvokeAsync(input, cancellationToken);

        mockMessageHandler.Verify(1);

        async Task OnHttpRequestAsync(HttpRequestMessage requestMessage)
        {
            Assert.Equal(HttpMethod.Post, requestMessage.Method);
            Assert.Equal(expectedRequestUri, requestMessage.RequestUri);

            Assert.NotNull(requestMessage.Content);

            var actualContent = await requestMessage.Content.ReadAsStringAsync();
            Assert.Equal(expectedContent, actualContent);
        }
    }

    [Theory]
    [MemberData(nameof(FirebaseSendFuncTestSource.OutputFailureTestData), MemberType = typeof(FirebaseSendFuncTestSource))]
    internal static async Task InvokeAsync_HttpResultIsFailure_ExpectFailure(
        HttpResponseMessage responseMessage, Failure<FirebaseSendFailureCode> expected)
    {
        using var mockMessageHandler = new MockHttpMessageHandler(responseMessage);
        var firebaseSendFunc = new ImplFirebaseSendFunc(mockMessageHandler, SomeProjectId);

        var cancellationToken = new CancellationToken(canceled: false);
        var actual = await firebaseSendFunc.InvokeAsync(SomeInput, cancellationToken);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(FirebaseSendFuncTestSource.OutputSuccessTestData), MemberType = typeof(FirebaseSendFuncTestSource))]
    public static async Task InvokeAsync_HttpResultIsSuccess_ExpectSuccess(
        HttpResponseMessage responseMessage)
    {
        using var mockMessageHandler = new MockHttpMessageHandler(responseMessage);
        var firebaseSendFunc = new ImplFirebaseSendFunc(mockMessageHandler, SomeProjectId);

        var cancellationToken = new CancellationToken(canceled: false);
        var actual = await firebaseSendFunc.InvokeAsync(SomeInput, cancellationToken);

        Assert.StrictEqual(Unit.Value, actual);
    }
}