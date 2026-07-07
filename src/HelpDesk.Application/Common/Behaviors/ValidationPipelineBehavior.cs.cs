using HelpDesk.Application.Tickets.Commands.CreateTicket;

namespace HelpDesk.Application.Common.Behaviors;

public sealed class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(error => error is not null)
            .ToList();

        if (failures.Count == 0)
        {
            return await next(cancellationToken); 
        }

        var firstFailure = failures[0];

        var error = new Error("Validation.Error", firstFailure.ErrorMessage, StatusCodes.Status400BadRequest);

        return CreateFailureResult(error);
    }

    private static TResponse CreateFailureResult(Error error)
    {
        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)Result.Failure(error);
        }

        var genericArgs = typeof(TResponse).GetGenericArguments();
        var genericResultType = typeof(Result<>).MakeGenericType(genericArgs);

        var failureMethod = typeof(Result)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(m =>
                m.Name == nameof(Result.Failure) &&
                m.IsGenericMethod &&
                m.GetParameters() is [{ ParameterType: var p }] && p == typeof(Error));

        var genericFailureMethod = failureMethod.MakeGenericMethod(genericArgs);

        return (TResponse)genericFailureMethod.Invoke(null, [error])!;
    }
}
