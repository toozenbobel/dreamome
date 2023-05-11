using System.Net;
using Common.Exceptions;
using Common.Models.Exceptions;

namespace Common.Filters;

public class CommonExceptionFilter
{
    public ExceptionHandleResult HandleException(Exception exception)
    {
        var handled = true;
        string exceptionKey;
        var status = default(HttpStatusCode);

        switch (exception)
        {
            case UnauthorizedAccessException _:
                exceptionKey = ExceptionKey.UnauthorizedAccess;
                status = HttpStatusCode.Unauthorized;
                break;
            case NotImplementedException _:
                exceptionKey = ExceptionKey.MethodNotImplemented;
                status = HttpStatusCode.NotImplemented;
                break;
            case ValidationException _:
            case FluentValidation.ValidationException:
                exceptionKey = ExceptionKey.RequestValidationFailure;
                status = HttpStatusCode.BadRequest;
                break;
            default:
                handled = false;
                exceptionKey = ExceptionKey.Unknown;
                break;
        }

        return new ExceptionHandleResult(handled, status, exceptionKey);
    }
}