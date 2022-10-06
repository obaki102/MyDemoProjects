using FluentValidation;
using MyDemoProjects.Application.Features.Authentication.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Features.Authentication.Validators;

public class LoginUserValidator : AbstractValidator<LoginUser>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Email)
          .NotEmpty().WithMessage("Your email cannot be empty")
          .EmailAddress().WithMessage("A valid email is required.")
          .Length(2, 100);
        RuleFor(p => p.Password).NotEmpty().WithMessage("Your password cannot be empty") ;
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<LoginUser>.CreateWithOptions((LoginUser)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}


