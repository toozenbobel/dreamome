using System.Net.Mime;
using Common.Base.Exceptions;
using Common.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace Common.Services.Exceptions;

public class ErrorHttpResponseBuilder : IErrorHttpResponseBuilder
{
    public async Task WriteErrorToResponse(HttpResponse httpResponse, ExceptionErrorDataModel error, ExceptionHandleResult handleResult)
    {
        httpResponse.StatusCode = (int)handleResult.HttpStatusCode;
        httpResponse.ContentType = MediaTypeNames.Application.Json;

        var result = JsonConvert.SerializeObject(error);
        Log.Error("{Error}", result);

        await httpResponse.WriteAsync(result);
    }
}