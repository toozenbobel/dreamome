using System.Net;
using Common.Base.Exceptions;
using Common.Exceptions;
using Common.Filters;
using Common.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Common.UnitTests.Filters;

public class ExceptionFilterDataSource
{
    public static IEnumerable<TestCaseData> TestCases => GetTestCases();

    private static IEnumerable<TestCaseData> GetTestCases()
    {
        yield return new TestCaseData(new UnauthorizedAccessException(),
            ExceptionKey.UnauthorizedAccess,
            HttpStatusCode.Unauthorized);
        yield return new TestCaseData(new NotImplementedException(),
            ExceptionKey.MethodNotImplemented,
            HttpStatusCode.NotImplemented);
        yield return new TestCaseData(new FluentValidation.ValidationException("Validation failure"),
            ExceptionKey.RequestValidationFailure,
            HttpStatusCode.BadRequest);
    }
}

public class CommonExceptionFilterTest
{
    [TestCaseSource(typeof(ExceptionFilterDataSource), nameof(ExceptionFilterDataSource.TestCases))]
    public void TestHandleException(System.Exception exception, string expectedKey, HttpStatusCode expectedStatusCode)
    {
        var expectedResult = new ExceptionHandleResult(true, expectedStatusCode, expectedKey);

        var filter = new CommonExceptionFilter();
        var result = filter.HandleException(exception);

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void TestHandleException_Unknown()
    {
        var expectedResult = new ExceptionHandleResult(false, default, ExceptionKey.Unknown);

        var filter = new CommonExceptionFilter();
        var result = filter.HandleException(new System.Exception());

        Assert.That(result, Is.EqualTo(expectedResult));
    }
}

public class GlobalExceptionFilterTest
{
    [Test]
    public async Task TestOnExceptionAsync_HandledByCommonExceptionFilter()
    {
        var commonExceptionFilter = new Mock<CommonExceptionFilter>();
        var httpResponseBuilder = new Mock<IErrorHttpResponseBuilder>();
        var errorDataModelFactory = new Mock<IExceptionErrorDataModelFactory>();

        var exception = new System.Exception();

        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>()) { Exception = exception };

        var errorDataModel = new ExceptionErrorDataModel
        {
            Key = "exceptionKey",
            Message = "Something bad happened",
            Type = "type"
        };

        var exceptionHandleResult = new ExceptionHandleResult(true, HttpStatusCode.BadRequest, errorDataModel.Key);

        commonExceptionFilter.Setup(x => x.HandleException(exception)).Returns(exceptionHandleResult);
        errorDataModelFactory.Setup(x => x.CreateErrorDataModel(context, exceptionHandleResult, exception)).Returns(errorDataModel);

        var filter = new GlobalExceptionFilter(commonExceptionFilter.Object, httpResponseBuilder.Object, errorDataModelFactory.Object);

        await filter.OnExceptionAsync(context);

        Assert.That(context.ExceptionHandled, Is.True);
        httpResponseBuilder
            .Verify(x => x.WriteErrorToResponse(context.HttpContext.Response, errorDataModel, exceptionHandleResult), Times.Once);
    }

    [Test]
    public async Task TestOnExceptionAsync_NotHandledByCommonExceptionFilter()
    {
        var commonExceptionFilter = new Mock<CommonExceptionFilter>();
        var httpResponseBuilder = new Mock<IErrorHttpResponseBuilder>();
        var errorDataModelFactory = new Mock<IExceptionErrorDataModelFactory>();

        var exception = new System.Exception();

        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>()) { Exception = exception };

        var errorDataModel = new ExceptionErrorDataModel
        {
            Key = "exceptionKey",
            Message = "Something bad happened",
            Type = "type"
        };

        var exceptionHandleResult = new ExceptionHandleResult(false, default, default!);

        var expectedHandleResult = exceptionHandleResult with
        {
            ExceptionKey = ExceptionKey.InternalServerError,
            HttpStatusCode = HttpStatusCode.InternalServerError
        };

        commonExceptionFilter.Setup(x => x.HandleException(exception)).Returns(exceptionHandleResult);
        errorDataModelFactory.Setup(x => x.CreateErrorDataModel(context, expectedHandleResult, exception)).Returns(errorDataModel);

        var filter = new GlobalExceptionFilter(commonExceptionFilter.Object, httpResponseBuilder.Object, errorDataModelFactory.Object);

        await filter.OnExceptionAsync(context);

        Assert.That(context.ExceptionHandled, Is.True);
        httpResponseBuilder
            .Verify(x => x.WriteErrorToResponse(context.HttpContext.Response, errorDataModel, expectedHandleResult), Times.Once);
    }

    [Test]
    public async Task TestOnExceptionAsync_UnexpectedError()
    {
        var commonExceptionFilter = new Mock<CommonExceptionFilter>();
        var httpResponseBuilder = new Mock<IErrorHttpResponseBuilder>();
        var errorDataModelFactory = new Mock<IExceptionErrorDataModelFactory>();

        var exception = new System.Exception();

        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>()) { Exception = exception };

        commonExceptionFilter.Setup(x => x.HandleException(exception)).Throws<System.Exception>();

        var filter = new GlobalExceptionFilter(commonExceptionFilter.Object, httpResponseBuilder.Object, errorDataModelFactory.Object);
        await filter.OnExceptionAsync(context);

        Assert.That(context.ExceptionHandled, Is.False);
    }
}