using System;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Platform.PushNotification.Push.Send.Test;

partial class PushSendHandlerTest
{
    [Theory]
    [MemberData(nameof(PushSendHandlerTestSource.InputInvalidTestData), MemberType = typeof(PushSendHandlerTestSource))]
    public static async Task InvokeAsync_InputIsInvalid_ExpectFailure(
        PushSendIn? input, Failure<HandlerFailureCode> expected)
    {
        var mockFirebaseFunc = BuildMockFirebaseSendFunc(Unit.Value);
        var mockTokenErrorBusApi = CreateMockTokenErrorBusApi();

        var handler = new PushSendHandler(mockFirebaseFunc.Object, mockTokenErrorBusApi.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        var actual = await handler.HandleAsync(input, cancellationToken);

        Assert.StrictEqual(expected, actual);

        mockTokenErrorBusApi.Verify(
            static a => a.SendMessageAsync(It.IsAny<BusMessageSendIn<PushTokenErrorJson>>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [MemberData(nameof(PushSendHandlerTestSource.InputValidTestData), MemberType = typeof(PushSendHandlerTestSource))]
    internal static async Task InvokeAsync_InputIsValid_ExpectFirebaseSendCalledOnce(
        PushSendIn input, FirebaseSendIn expectedInput)
    {
        var mockFirebaseFunc = BuildMockFirebaseSendFunc(Unit.Value);
        var mockTokenErrorBusApi = CreateMockTokenErrorBusApi();

        var handler = new PushSendHandler(mockFirebaseFunc.Object, mockTokenErrorBusApi.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await handler.HandleAsync(input, cancellationToken);

        mockFirebaseFunc.Verify(f => f.InvokeAsync(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [InlineData(FirebaseSendFailureCode.Unknown)]
    [InlineData(FirebaseSendFailureCode.InvalidArgument)]
    [InlineData(FirebaseSendFailureCode.Unregistered)]
    [InlineData(FirebaseSendFailureCode.SenderIdMismatch)]
    [InlineData(FirebaseSendFailureCode.QuotaExceeded)]
    [InlineData(FirebaseSendFailureCode.Unavailable)]
    [InlineData(FirebaseSendFailureCode.Internal)]
    [InlineData(FirebaseSendFailureCode.AuthorizationError)]
    internal static async Task InvokeAsync_FirebaseResultIsNotInvalidArgumentFailure_ExpectPushTokenErrorBusApiCalledNever(
        FirebaseSendFailureCode firebaseFailureCode)
    {
        var sourceException = new Exception("Some Error Message");
        var firebaseFailure = sourceException.ToFailure(firebaseFailureCode, "Some Failure message");

        var mockFirebaseFunc = BuildMockFirebaseSendFunc(firebaseFailure);
        var mockTokenErrorBusApi = CreateMockTokenErrorBusApi();

        var handler = new PushSendHandler(mockFirebaseFunc.Object, mockTokenErrorBusApi.Object);

        _ = await handler.HandleAsync(SomeInput, default);

        mockTokenErrorBusApi.Verify(
            static a => a.SendMessageOrFailureAsync(It.IsAny<BusMessageSendIn<PushTokenErrorJson>>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Theory]
    [InlineData(FirebaseSendFailureCode.Unregistered)]
    [InlineData(FirebaseSendFailureCode.InvalidArgument)]
    internal static async Task InvokeAsync_FirebaseResultIsInvalidArgumentFailureAndErrorBusApiIsNptNull_ExpectPushTokenErrorBusApiCalledOnce(
        FirebaseSendFailureCode firebaseFailureCode)
    {
        var sourceException = new Exception("Some error Message");
        var firebaseFailure = sourceException.ToFailure(firebaseFailureCode, "Some failure message");

        var mockFirebaseFunc = BuildMockFirebaseSendFunc(firebaseFailure);
        var mockTokenErrorBusApi = CreateMockTokenErrorBusApi();

        var handler = new PushSendHandler(mockFirebaseFunc.Object, mockTokenErrorBusApi.Object);

        var input = new PushSendIn(
            token: "Some push token",
            notification: new()
            {
                Title = "Some Title",
                Body = "Some body"
            },
            data: null);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await handler.HandleAsync(input, cancellationToken);

        var expectedInput = new BusMessageSendIn<PushTokenErrorJson>(
            message: new()
            {
                PushToken = "Some push token"
            });

        mockTokenErrorBusApi.Verify(a => a.SendMessageAsync(expectedInput, cancellationToken), Times.Once);
    }

    [Theory]
    [MemberData(nameof(PushSendHandlerTestSource.OutputFailureTestData), MemberType = typeof(PushSendHandlerTestSource))]
    internal static async Task InvokeAsync_FirebaseResultIsFailure_ExpectFailure(
        IBusMessageSendSupplier<PushTokenErrorJson>? errorBusApi, FirebaseSendFailureCode firebaseFailureCode, HandlerFailureCode expectedFailureCode)
    {
        var sourceException = new Exception("Some error Message");
        var firebaseFailure = sourceException.ToFailure(firebaseFailureCode, "Some failure message");

        var mockFirebaseFunc = BuildMockFirebaseSendFunc(firebaseFailure);
        var handler = new PushSendHandler(mockFirebaseFunc.Object, errorBusApi);

        var actual = await handler.HandleAsync(SomeInput, default);
        var expected = Failure.Create(expectedFailureCode, "Some failure message", sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task InvokeAsync_FirebaseResultIsSuccess_ExpectSuccess()
    {
        var mockFirebaseFunc = BuildMockFirebaseSendFunc(Unit.Value);
        var mockTokenErrorBusApi = CreateMockTokenErrorBusApi();

        var handler = new PushSendHandler(mockFirebaseFunc.Object, mockTokenErrorBusApi.Object);

        var actual = await handler.HandleAsync(SomeInput, default);
        var expected = Result.Success<Unit>(default);

        Assert.StrictEqual(expected, actual);
    }
}