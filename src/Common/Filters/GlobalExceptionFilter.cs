using System.Net;
using Common.Base.Exceptions;
using Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Common.Filters;

public class GlobalExceptionFilter : ExceptionFilterAttribute
{
    private readonly CommonExceptionFilter _commonExceptionFilter;
    private readonly IExceptionErrorDataModelFactory _errorDataModelFactory;
    private readonly IErrorHttpResponseBuilder _errorHttpResponseBuilder;

    public GlobalExceptionFilter(CommonExceptionFilter commonExceptionFilter,
        IErrorHttpResponseBuilder errorHttpResponseBuilder,
        IExceptionErrorDataModelFactory errorDataModelFactory)
    {
        _commonExceptionFilter = commonExceptionFilter;
        _errorHttpResponseBuilder = errorHttpResponseBuilder;
        _errorDataModelFactory = errorDataModelFactory;
    }

    public override async Task OnExceptionAsync(ExceptionContext context)
    {
        try
        {
            var ex = context.Exception;

            var handleResult = _commonExceptionFilter.HandleException(ex);
            if (handleResult.IsHandled == false)
                handleResult = handleResult with
                {
                    ExceptionKey = ExceptionKey.InternalServerError,
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };

            context.ExceptionHandled = true;

            Log.Error(ex, "ErrorType: {ErrorType}", ex.GetType().Name);

            var errorDataModel = _errorDataModelFactory.CreateErrorDataModel(context, handleResult, ex);
            await _errorHttpResponseBuilder.WriteErrorToResponse(context.HttpContext.Response, errorDataModel, handleResult);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred in a global exception filter!");
        }
    }
}