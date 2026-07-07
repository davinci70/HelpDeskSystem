using HelpDesk.Application.Common.Results;

namespace HelpDesk.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
