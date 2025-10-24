using eSchoolDatabase.Models;
using eSchoolDatabase.RequestModel;
using FluentValidation;

namespace eSchoolProject.Components.PopupComponent.Validator
{
    public class ManagerValidator : AbstractValidator<ManagerRequestModel>
    {
        public ManagerValidator()
        {
            RuleFor(a => a.Name)
                .NotNull().WithMessage("Name is required.")
                .NotEmpty().WithMessage("Name cannot be empty.")
                .Matches("^[a-zA-ZçÇğĞıİöÖşŞüÜ\\s]+$").WithMessage("Name must contain only letters.");

            RuleFor(a => a.IdentityNumber)
                .NotNull().WithMessage("IdentityNumber is required.")
                .NotEmpty().WithMessage("IdentityNumber cannot be empty.")
                .Matches("^[0-9]+$").WithMessage("IdentityNumber must contain only numbers.")
                .Length(11).WithMessage("IdentityNumber must be exactly 11 digits.");
           
            RuleFor(a => a.PhoneNumber)
                .NotNull().WithMessage("Phone number is required")
                .NotEmpty().WithMessage("Phone number cannot be empty")
                .Matches("^[0-9]+$").WithMessage("Phone number must contain only numbers.")
                .Length(11).WithMessage("IdentityNumber must be exactly 11 digits.");
        }
    }
}
