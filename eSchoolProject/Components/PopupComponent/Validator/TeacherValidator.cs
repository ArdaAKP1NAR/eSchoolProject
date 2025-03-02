using eSchoolDatabase.RequestModels;
using FluentValidation;

namespace eSchoolProject.Components.PopupComponent.Validator
{
    public class TeacherValidator : AbstractValidator<TeacherRequestModel>
    {
        public TeacherValidator()
        {
            RuleFor(a => a.Name)
                      .NotNull().WithMessage("Name is required.")
                      .NotEmpty().WithMessage("Name cannot be empty.")
                      .NotEmpty().WithMessage("Name cannot be empty.")
                      .Matches("^[a-zA-ZçÇğĞıİöÖşŞüÜ\\s]+$").WithMessage("Name must contain only letters.");
            
            RuleFor(a => a.IdentityNumber)
                .NotNull().WithMessage("IdentityNumber is required.")
                .NotEmpty().WithMessage("IdentityNumber cannot be empty.")
                .Matches("^[0-9]+$").WithMessage("IdentityNumber must contain only numbers.")
                .Length(11).WithMessage("IdentityNumber must be exactly 11 digits.");

            RuleFor(a => a.Password)
                .NotNull().WithMessage("Password is required.")
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W]").WithMessage("Password must contain at least one special character (!@#$%^&* etc.).");

            RuleFor(a => a.PhoneNumber)
                .NotNull().WithMessage("Phone number is required")
                .NotEmpty().WithMessage("Phone number cannot be empty")
                .Matches("^[0-9]+$").WithMessage("Phone number must contain only numbers.")
                .Length(11).WithMessage("IdentityNumber must be exactly 11 digits.");
        }
    }
}