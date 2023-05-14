using Common.Models.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Common.Base.Exceptions;

public interface IErrorHttpResponseBuilder
{
    Task WriteErrorToResponse(HttpResponse httpResponse, ExceptionErrorDataModel error, ExceptionHandleResult handleResult);
}