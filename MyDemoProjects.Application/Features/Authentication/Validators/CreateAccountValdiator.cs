using MyDemoProjects.Application.Features.Authentication.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Features.Authentication.Validators
{
    public class CreateAccountValdiator : AbstractValidator<CreateAccount>
    {
        public CreateAccountValdiator()
        {
            RuleFor(x => x.User.Email)
             .NotEmpty().WithMessage("Your email cannot be empty.")
             .EmailAddress().WithMessage("A valid email is required.")
             .Length(2, 100);

            RuleFor(x => x.User.UserName)
                    .NotEmpty().WithMessage("Your username cannot be empty.");

            RuleFor(x => x.User.FirstName)
                 .NotEmpty().WithMessage("Your first name cannot be empty.");

            RuleFor(x => x.User.LastName)
                 .NotEmpty().WithMessage("Your last name cannot be empty.");

            RuleFor(x => x.User.Password).NotEmpty().WithMessage("Your password cannot be empty")
                      .MinimumLength(6).WithMessage("Your password length must be at least 6.")
                      .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                      .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                      .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                      .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                      .Matches(@"[\@\!\?\*\.]+").WithMessage("Your password must contain at least one (@!? *.).");
            RuleFor(x => x.User.ConfirmPassword)
                .Equal(x => x.User.Password).WithMessage("Passwords don't match");
        }
    }
}
