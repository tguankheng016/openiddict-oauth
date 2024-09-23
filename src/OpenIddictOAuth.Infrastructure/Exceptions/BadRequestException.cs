namespace OpenIddictOAuth.Infrastructure.Exceptions;

public class BadRequestException : CustomException
{
    public BadRequestException(string message, int? code = null) : base(message, code: code)
    {
    }
}