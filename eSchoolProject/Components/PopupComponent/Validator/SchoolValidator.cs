using eSchoolDatabase.RequestModel;
using FluentValidation;

namespace eSchoolProject.Components.PopupComponent.Validator
{
    public class SchoolValidator : AbstractValidator<SchoolRequestModel>
    {
        public SchoolValidator()
        {
            RuleFor(a => a.Name)
                .NotNull().WithMessage("Name is required.")
                .NotEmpty().WithMessage("Name cannot be empty.")
                .Matches("^[a-zA-ZçÇğĞıİöÖşŞüÜ\\s]+$").WithMessage("Name must contain only letters.");


            RuleFor(a => a.Address.City)
                .NotNull().WithMessage("City is required.")
                .NotEmpty().WithMessage("City cannot be empty.")
                .Matches("^[a-zA-ZçÇğĞıİöÖşŞüÜ\\s]+$").WithMessage("City must contain only letters.");

            RuleFor(a => a.Address.District)
                .NotNull().WithMessage("District is required.")
                .NotEmpty().WithMessage("District cannot be empty.")
                .Matches("^[a-zA-ZçÇğĞıİöÖşŞüÜ\\s]+$").WithMessage("District must contain only letters.");

            RuleFor(a => a.Address.Street)
                .NotNull().WithMessage("Street is required.")
                .Matches("^[a-zA-ZçÇğĞıİöÖşŞüÜ\\s]+$").WithMessage("Street must contain only letters.");
        }
    }
}
