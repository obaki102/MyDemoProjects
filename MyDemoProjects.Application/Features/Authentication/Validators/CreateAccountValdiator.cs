
namespace MyDemoProjects.Application.Features.Authentication.Validators
{
    public class CreateAccountValdiator : AbstractValidator<CreateAccount>
    {
        public CreateAccountValdiator()
        {
            RuleFor(x => x.Email)
             .NotEmpty().WithMessage("Your email cannot be empty.")
             .EmailAddress().WithMessage("A valid email is required.")
             .Length(2, 100);

            RuleFor(x => x.UserName)
                    .NotEmpty().WithMessage("Your username cannot be empty.");

            RuleFor(x => x.FirstName)
                 .NotEmpty().WithMessage("Your first name cannot be empty.");

            RuleFor(x => x.LastName)
                 .NotEmpty().WithMessage("Your last name cannot be empty.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Your password cannot be empty")
                      .MinimumLength(6).WithMessage("Your password length must be at least 6.")
                      .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                      .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                      .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                      .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                      .Matches(@"[\@\!\?\*\.]+").WithMessage("Your password must contain at least one (@!? *.).");
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords don't match");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<CreateAccount>.CreateWithOptions((CreateAccount)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
