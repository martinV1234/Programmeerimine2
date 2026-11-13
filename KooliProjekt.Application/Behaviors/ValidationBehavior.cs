using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
namespace KooliProjekt.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TResponse : OperationResult, new()
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) { _validators = validators; }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            var response = new TResponse();
            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(new ValidationContext<TRequest>(request), ct);
                foreach (var error in result.Errors) response.AddPropertyError(error.PropertyName, error.ErrorMessage);
            }
            return response.HasErrors ? response : await next();
        }
    }
}
