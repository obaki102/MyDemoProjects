using Duende.IdentityServer.Models;
using FluentValidation;
using MyDemoProjects.Application.Shared.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Behaviours.Validation
{
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }
            var context = new ValidationContext<TRequest>(request);

            //var errorsDictionary = _validators
            //    .Select(x => x.Validate(context))
            //    .SelectMany(x => x.Errors)
            //    .Where(x => x != null)
            //    .GroupBy(
            //        x => x.PropertyName,
            //        x => x.ErrorMessage,
            //        (propertyName, errorMessages) => new
            //        {
            //            Key = propertyName,
            //            Values = errorMessages.Distinct().ToArray()
            //        })
            //    .ToDictionary(x => x.Key, x => x.Values);

            var errorLists = _validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .Select(x => x.ErrorMessage)
                .ToList();

            if (errorLists.Any())
            {
                //TODO: Check other ways to throw the validation error messages
                //Check if respons is an application response type
                if (typeof(TResponse) is IApplicationResponse iapplicationResponse)
                {
                    //Build the parameter to be pass
                    var parameterType = new Type[] { typeof(List<string>) };
                    //Find the method "fail" in Application Response object
                    var failMethodInApplicationResponse = typeof(TResponse).GetMethod("Fail", parameterType);
                    //Create the Application Response object
                    var responseObject = (TResponse)Activator.CreateInstance(typeof(TResponse));
                    //Pass the list of errors as a parameter
                    object[] parameters = new object[] { errorLists };
                    return (TResponse)failMethodInApplicationResponse.Invoke(responseObject, parameters);
                }
            }
                                                    
            return await next();
        }
    }
}
