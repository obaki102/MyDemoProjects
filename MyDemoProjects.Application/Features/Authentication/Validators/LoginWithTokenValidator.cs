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
        RuleFor(x => x.User.Email)
          .NotEmpty().WithMessage("Your email cannot be empty")
          .EmailAddress().WithMessage("A valid email is required.")
          .Length(2, 100);
        RuleFor(p => p.User.Password).NotEmpty().WithMessage("Your password cannot be empty") ;
    }
}


