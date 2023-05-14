using System.Net;
using Common.Models.Exceptions;
using Common.Services.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Common.UnitTests.Services.Exceptions;

public class ExceptionErrorDataModelFactoryTest
{
    [Test]
    public void TestCreateErrorDataModel_Production()
    {
        var env = new Mock<IWebHostEnvironment>();
        env.Setup(x => x.EnvironmentName).Returns("Production");

        var exception = new InvalidOperationException();

        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        var resultExecutingContext = new ExceptionContext(actionContext, new List<IFilterMetadata>()) { Exception = exception };

        var handleResult = new ExceptionHandleResult(true, HttpStatusCode.InternalServerError, "exceptionKey");

        var expectedResult = new ExceptionErrorDataModel
        {
            Type = exception.GetType().Name,
            Key = handleResult.ExceptionKey
        };

        var factory = new ExceptionErrorDataModelFactory(env.Object);
        var result = factory.CreateErrorDataModel(resultExecutingContext, handleResult, exception);

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void TestCreateErrorDataModel_Development()
    {
        var env = new Mock<IWebHostEnvironment>();
        env.Setup(x => x.EnvironmentName).Returns("Development");

        var exception = new InvalidOperationException("Something went wrong");

        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor()
        };

        var resultExecutingContext = new ExceptionContext(actionContext, new List<IFilterMetadata>()) { Exception = exception };

        var handleResult = new ExceptionHandleResult(true, HttpStatusCode.InternalServerError, "exceptionKey");

        var expectedResult = new ExceptionErrorDataModel
        {
            Type = exception.GetType().Name,
            Key = handleResult.ExceptionKey,
            Message = exception.Message,
            Stack = exception.StackTrace?.Split("\n")
        };

        var factory = new ExceptionErrorDataModelFactory(env.Object);
        var result = factory.CreateErrorDataModel(resultExecutingContext, handleResult, exception);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Type, Is.EqualTo(expectedResult.Type));
            Assert.That(result.Key, Is.EqualTo(expectedResult.Key));
            Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
        });
    }
}