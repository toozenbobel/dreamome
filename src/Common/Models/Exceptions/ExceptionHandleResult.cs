using System.Net;

namespace Common.Models.Exceptions;

public record ExceptionHandleResult(bool IsHandled, HttpStatusCode HttpStatusCode, string ExceptionKey);