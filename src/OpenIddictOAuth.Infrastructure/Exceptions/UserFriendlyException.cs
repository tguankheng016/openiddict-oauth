namespace OpenIddictOAuth.Infrastructure.Exceptions;

public class UserFriendlyException : CustomException
{
    public string? Details { get; private set; }

    public UserFriendlyException(string message, string? details = null, int? code = null)
        : base(message, code: code)
    {
        Details = details;
    }
}