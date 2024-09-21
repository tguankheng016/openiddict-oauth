using MediatR;

namespace OpenIddictOAuth.Infrastructure.CQRS;

public interface IQuery<out T> : IRequest<T>
    where T : notnull
{
}