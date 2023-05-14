using Common.Models.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.Base.Exceptions;

public interface IExceptionErrorDataModelFactory
{
    ExceptionErrorDataModel CreateErrorDataModel(ExceptionContext context, ExceptionHandleResult handleResult, Exception ex);
}