using MediatR;
using OpenIddictOAuth.Infrastructure.EfCore;

namespace OpenIddictOAuth.Infrastructure.CQRS;

public interface ICommand : ICommand<Unit>
{
}

// Transactional by default
public interface ICommand<out T> : IRequest<T>, ITransactional
    where T : notnull
{
}