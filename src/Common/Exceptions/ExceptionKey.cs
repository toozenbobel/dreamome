namespace Common.Exceptions;

public static class ExceptionKey
{
    public const string UnauthorizedAccess = "Unauthorized_Access";
    public const string ServiceBusy = "Service_Busy";
    public const string ServerErrorOccured = "A_Server_Error_Occurred";
    public const string InvalidEntry = "Invalid_Entry";
    public const string RequestValidationFailure = "Request_Validation_Failure";
    public const string EntityNotFound = "Entity_Not_Found";
    public const string InfrastructureError = "Infrastructure_Error";
    public const string InternalServerError = "InternalServerError (report to a program administrator)";
    public const string MethodNotImplemented = "Method_Is_Not_Implemented";
    public const string Unknown = "Unknown";
}