using Common.Base.Exceptions;
using Common.Models.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace Common.Services.Exceptions;

public class ExceptionErrorDataModelFactory : IExceptionErrorDataModelFactory
{
    private readonly IWebHostEnvironment _env;

    public ExceptionErrorDataModelFactory(IWebHostEnvironment webHostEnvironment)
    {
        _env = webHostEnvironment;
    }

    public ExceptionErrorDataModel CreateErrorDataModel(ExceptionContext context, ExceptionHandleResult handleResult, Exception ex)
    {
        var exceptionType = ex.GetType();
        var error = new ExceptionErrorDataModel
        {
            Key = handleResult.ExceptionKey,
            Type = exceptionType.Name
        };

        if (_env.IsDevelopment())
        {
            error = error with
            {
                Stack = context.Exception.StackTrace?.Split("\n") ?? Array.Empty<string>(),
                Message = context.Exception.Message
            };
        }

        return error;
    }
}