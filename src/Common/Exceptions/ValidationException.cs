using FluentValidation.Results;

namespace Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base(string.Join("; ", failures), null) { }
}