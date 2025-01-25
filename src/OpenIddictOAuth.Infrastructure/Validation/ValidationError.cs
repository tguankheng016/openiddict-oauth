namespace OpenIddictOAuth.Infrastructure.Validation;

public class ValidationError
{
    public string Field { get; } = "";

    public string Message { get; } = "";

    public ValidationError(string field, string message)
    {
        Field = field != string.Empty ? field : "";
        Message = message;
    }
}