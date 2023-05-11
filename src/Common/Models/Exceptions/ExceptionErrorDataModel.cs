namespace Common.Models.Exceptions;

public record ExceptionErrorDataModel
{
    public required string Key { get; init; }
    public required string Type { get; init; }

    public string? Message { get; init; }

    public string[]? Stack { get; init; }
}