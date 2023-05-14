using System.Collections;
using MediatR;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace UnitTestsCommon;

public static class MediatorMockingHelper
{
    public static void MockRequestRecord<TRequest, TResponse>(this Mock<IMediator> mediatorMock,
        TRequest request,
        TResponse response) where TRequest : IRequest<TResponse>
    {
        mediatorMock.Setup(x => x.Send(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response)
            .Verifiable();
    } 
    
    public static void MockRequest<TRequest, TResponse>(this Mock<IMediator> mediatorMock,
        TRequest request,
        TResponse response) where TRequest : IRequest<TResponse>
    {
        mediatorMock.Setup(x => x.Send(It.Is<TRequest>(r => CheckProperties(r, request)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response)
            .Verifiable();
    }

    public static void MockRequest<TRequest>(this Mock<IMediator> mediatorMock,
        TRequest request) where TRequest : IRequest
    {
        mediatorMock.Setup(x => x.Send(It.Is<TRequest>(r => CheckProperties(r, request)), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
    }

    public static void MockRequestException<TRequest, TResponse>(this Mock<IMediator> mediatorMock,
        TRequest request,
        Exception exception) where TRequest : IRequest<TResponse>
    {
        mediatorMock.Setup(x => x.Send(It.Is<TRequest>(r => CheckProperties(r, request)), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception)
            .Verifiable();
    }

    public static void MockRequestException<TRequest>(this Mock<IMediator> mediatorMock,
        TRequest request,
        Exception exception) where TRequest : IRequest
    {
        mediatorMock.Setup(x => x.Send(It.Is<TRequest>(r => CheckProperties(r, request)), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception)
            .Verifiable();
    }

    public static void MockHandler<TRequest, TResponse>(this Mock<IMediator> mediatorMock,
        IRequestHandler<TRequest, TResponse> handler,
        Action<TResponse> responseCallback = null) where TRequest : IRequest<TResponse>
    {
        mediatorMock.Setup(x => x.Send(It.IsAny<TRequest>(), CancellationToken.None))
            .Returns<TRequest, CancellationToken>(async (r, _) =>
            {
                var response = await handler.Handle(r, _);
                responseCallback?.Invoke(response);
                return response;
            });
    }

    public static void MockHandler<TRequest>(this Mock<IMediator> mediatorMock, IRequestHandler<TRequest> handler)
        where TRequest : IRequest
    {
        mediatorMock.Setup(x => x.Send(It.IsAny<TRequest>(), CancellationToken.None))
            .Returns<TRequest, CancellationToken>(async (r, _) => { await handler.Handle(r, _); });
    }

    private static bool CheckProperties<TRequest>(TRequest actualRequest, TRequest expectedRequest)
    {
        var actualProperties = actualRequest.GetType().GetProperties();
        var expectedProperties = expectedRequest.GetType().GetProperties();

        foreach (var expectedProperty in expectedProperties)
        {
            var actualProperty = actualProperties.FirstOrDefault(x => x.Name == expectedProperty.Name);
            if (actualProperty is null)
                throw new Exception($"Property {expectedProperty.Name} not found in request {typeof(TRequest)}");

            var actualValue = actualProperty.GetValue(actualRequest);
            var expectedValue = expectedProperty.GetValue(expectedRequest);

            var tolerance = Tolerance.Default;
            var comparer = new NUnitEqualityComparer();
            if (comparer.AreEqual(actualValue, expectedValue, ref tolerance))
                continue;

            if (expectedValue is IEnumerable expectedValueEnumerable
                && actualValue is IEnumerable actualValueEnumerable)
            {
                var actualEnumerator = actualValueEnumerable.GetEnumerator();
                var expectedEnumerator = expectedValueEnumerable.GetEnumerator();

                while (expectedEnumerator.MoveNext())
                {
                    if (actualEnumerator.MoveNext() == false)
                        Assert.Fail("Enumeration returned less results");

                    CheckProperties(actualEnumerator.Current, expectedEnumerator.Current);
                }
            }
            else if (expectedValue is not null)
            {
                Assert.That(actualValue, Is.EqualTo(expectedValue), $"{actualProperty.Name} equality check failed");
            }
            else
            {
                Assert.That(actualValue, Is.Null, $"{actualProperty.Name} equality check failed");
            }
        }

        return true;
    }
}