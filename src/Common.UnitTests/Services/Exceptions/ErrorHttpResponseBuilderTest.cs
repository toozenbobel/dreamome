using System.Net;
using System.Net.Mime;
using Common.Models.Exceptions;
using Common.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Common.UnitTests.Services.Exceptions;

public class ErrorHttpResponseBuilderTest
{
    [Test]
    public async Task TestWriteErrorToResponse()
    {
        var model = new ExceptionErrorDataModel
        {
            Key = "exceptionKey",
            Message = "Something went wrong",
            Type = "type"
        };

        var httpContext = new DefaultHttpContext { Response = { Body = new MemoryStream() } };
        var handleResult = new ExceptionHandleResult(true, HttpStatusCode.InternalServerError, model.Key);

        var expectedJson = JsonConvert.SerializeObject(model);

        var builder = new ErrorHttpResponseBuilder();
        await builder.WriteErrorToResponse(httpContext.Response, model, handleResult);

        httpContext.Response.Body.Position = 0;
        using (var reader = new StreamReader(httpContext.Response.Body))
        {
            var result = await reader.ReadToEndAsync();
            Assert.That(result, Is.EqualTo(expectedJson));
        }

        Assert.Multiple(() =>
        {
            Assert.That(httpContext.Response.StatusCode, Is.EqualTo((int)handleResult.HttpStatusCode));
            Assert.That(httpContext.Response.ContentType, Is.EqualTo(MediaTypeNames.Application.Json));
        });
    }
}