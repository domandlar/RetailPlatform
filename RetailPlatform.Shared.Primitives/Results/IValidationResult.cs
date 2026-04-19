namespace RetailPlatform.Shared.Primitives.Results;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        ErrorType.ValidationFailure,
        "ValidationError",
        "A validation problem occurred.");

    Error[] Errors { get; }
}
