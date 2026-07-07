using HelpDesk.Application.Common.Results;

namespace HelpDesk.Application.Abstractions.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
