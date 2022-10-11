using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Features.Authentication.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePassword>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("Email Address cannot be empty")
                 .EmailAddress().WithMessage("A valid email is required.")
                 .Length(2, 100);

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Password cannot be empty");

            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Your password cannot be empty")
                     .MinimumLength(6).WithMessage("Your password length must be at least 6.")
                     .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                     .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                     .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                     .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                     .Matches(@"[\@\!\?\*\.]+").WithMessage("Your password must contain at least one (@!? *.).");
        }
    }
}
