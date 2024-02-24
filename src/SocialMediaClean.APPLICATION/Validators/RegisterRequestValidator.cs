using FluentValidation;
using SocialMediaClean.APPLICATION.Requests;

namespace SocialMediaClean.APPLICATION.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator() 
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                    .WithMessage("Invalid email!")
                .NotEmpty()
                    .WithMessage("Invalid email!");
            RuleFor(x => x.FirstName)
                .NotEmpty()
                    .WithMessage("First name cannot be empty!");
            RuleFor(x => x.LastName)
                .NotEmpty()
                    .WithMessage("Last name cannot be empty!");
            
                
        

        }
        private bool BeValidGender(string gender)
        {
            if (string.IsNullOrEmpty(gender))
            {
                return false;
            }
            return gender.Equals("M") || gender.Equals("F");
        }
    }
}
