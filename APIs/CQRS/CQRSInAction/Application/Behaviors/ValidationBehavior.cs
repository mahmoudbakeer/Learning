using FluentValidation;
using MediatR;

namespace CQRSInAction.Application.Behaviors;



public class ValidationBehaviors<TRequest, TResponse>(IEnumerable<IValidator> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next(cancellationToken);


        var context = new ValidationContext<TRequest>(request);

        var result = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = result.SelectMany(r => r.Errors).Where(e => e is not null);


        if (failures.Any()) throw new ValidationException(failures);
        return await next(cancellationToken);
    }
}