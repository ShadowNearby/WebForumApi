using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebForumApi.Application.Common.Behaviors;

public class ValidationResultPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationResultPipelineBehavior(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        IValidator<TRequest>? validator = _serviceProvider.GetService<IValidator<TRequest>>();

        if (validator != null)
        {
            ValidationResult? result = await validator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                // Reference: https://github.com/amantinband/error-or/issues/10
                /* Due to not wanting to use reflection, we assume that every request
                 * that wants to validate something also returns a result.
                 * Using implicit casts, we are able to use this same behavior for all of them
                 */
                return (TResponse)(dynamic)Result.Invalid(result.AsErrors());
            }
        }

        return await next();
    }
}