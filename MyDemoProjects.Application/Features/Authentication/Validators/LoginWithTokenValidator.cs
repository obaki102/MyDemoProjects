using FluentValidation;
using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Shared.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Features.Authentication.Validators;

public class LoginWithTokenValidator : AbstractValidator<LoginWithToken>
{
    public LoginWithTokenValidator()
    {
        RuleFor(x => x.User.EmailAddress)
          .NotEmpty().WithMessage("Your email cannot be empty")
          .EmailAddress().WithMessage("A valid email is required.")
          .Length(2, 100);
        RuleFor(p => p.User.Password).NotEmpty().WithMessage("Your password cannot be empty")
                  .MinimumLength(6).WithMessage("Your password length must be at least 6.")
                  .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                  .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                  .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                  .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                  .Matches(@"[\@\!\?\*\.]+").WithMessage("Your password must contain at least one (@!? *.).");
    }
}


