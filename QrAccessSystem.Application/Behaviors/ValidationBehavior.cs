using FluentValidation;
using MediatR;

namespace QrAccessSystem.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            
            // Tüm kural hatalarını tek bir listede topluyoruz
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                // Hataları listeye çevirip kendi özel Exception'ımıza fırlatıyoruz
                var errorMessages = failures.Select(x => x.ErrorMessage).ToList();
                throw new Exceptions.ValidationException(errorMessages);
            }
        }
        // Hata yoksa yola devam et (Handler'a git)
        return await next();
    }
}