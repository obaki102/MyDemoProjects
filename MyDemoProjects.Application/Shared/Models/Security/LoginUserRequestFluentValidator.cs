using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Shared.Models.Security;

public class LoginUserRequestFluentValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestFluentValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Your email cannot be empty")
            .EmailAddress().WithMessage("A valid email is required.")
            .Length(2, 100);
        RuleFor(p => p.Password).NotEmpty().WithMessage("Your password cannot be empty")
                  .MinimumLength(6).WithMessage("Your password length must be at least 6.")
                  .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                  .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                  .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                  .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                  .Matches(@"[\@\!\?\*\.]+").WithMessage("Your password must contain at least one (@!? *.).");

    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<LoginUserRequest>.CreateWithOptions((LoginUserRequest)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
