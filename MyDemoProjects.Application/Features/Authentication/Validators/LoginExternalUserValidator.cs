using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MyDemoProjects.Application.Features.Authentication.Validators
{
    public class LoginExternalUserValidator : AbstractValidator<LoginExternalUser>
    {
        public LoginExternalUserValidator()
        {
            RuleFor(x => x.EmailAddress)
                 .NotEmpty().WithMessage("Your email address cannot be empty.");
            RuleFor(x => x.Provider)
               .NotEmpty().WithMessage("Your provider cannot be empty.");
            RuleFor(x => x.AccessToken)
              .NotEmpty().WithMessage("Your token cannot be empty.");
        }
    }
}
