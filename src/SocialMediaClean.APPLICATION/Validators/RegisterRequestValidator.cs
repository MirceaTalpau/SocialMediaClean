using FluentValidation;
using SocialMediaClean.APPLICATION.Requests;

namespace SocialMediaClean.APPLICATION.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator() 
        {
            RuleFor(x => x.BirthDay)
                .NotEmpty()
                .WithMessage("Birthday cannot be empty!");
            RuleFor(x => x.Email)
                .EmailAddress()
                    .WithMessage("Invalid email!")
                .NotEmpty()
                    .WithMessage("Invalid email!");
            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("Password cannot be empty!");
            RuleFor(x => x.FirstName)
                .NotEmpty()
                    .WithMessage("First name cannot be empty!");
            RuleFor(x => x.LastName)
                .NotEmpty()
                    .WithMessage("Last name cannot be empty!");
            RuleFor(x => x.Gender)
                .Must(BeValidGender)
                    .WithMessage("Gender must be either 'M' or 'F'");
                
        

        }
        private bool BeValidGender(string gender)
        {
            return gender.Equals("M") || gender.Equals("F");
        }
    }
}
