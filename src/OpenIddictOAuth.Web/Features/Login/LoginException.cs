using OpenIddictOAuth.Infrastructure.Exceptions;

namespace OpenIddictOAuth.Web.Features.Login;

public class InvalidUserException : BadRequestException
{
    public InvalidUserException(string message = "Invalid username or password!", int? code = null) : base(message, code)
    {
    }
}