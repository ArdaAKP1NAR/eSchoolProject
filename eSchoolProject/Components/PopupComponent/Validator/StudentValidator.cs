using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using FluentValidation;

namespace eSchoolProject.Components.PopupComponent.Validator
{
    public class StudentValidator : AbstractValidator<StudentViewModel>
    {
        public StudentValidator()
        {
            // Name Validation
            RuleFor(a => a.Name)
                .NotNull().WithMessage("Name is required.")
                .NotEmpty().WithMessage("Name cannot be empty.")
                .Matches("^[a-zA-ZçÇğĞıİöÖşŞüÜ\\s]+$").WithMessage("Name must contain only letters.");

            // IdentityNumber Validation
            RuleFor(a => a.IdentityNumber)
                .NotNull().WithMessage("IdentityNumber is required.")
                .NotEmpty().WithMessage("IdentityNumber cannot be empty.")
                .Matches("^[0-9]+$").WithMessage("IdentityNumber must contain only numbers.")
                .Length(11).WithMessage("IdentityNumber must be exactly 11 digits.");



            RuleFor(a => a.StudentNumber)
                .NotNull().WithMessage("Student number is required.")
                .NotEmpty().WithMessage("Student number cannot be empty.")
                .GreaterThan(0).WithMessage("Student number must be greater than zero.");

            RuleFor(a => a.BirthdayDate)
                .LessThan(DateTime.Now).WithMessage("Birthday date must be in the past.")
                .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Birthday date is not valid.");

            // ParentNumber Validation
            RuleFor(a => a.ParentNumber)
                .NotNull().WithMessage("Parent number is required.")
                .NotEmpty().WithMessage("Parent number cannot be empty.")
                .Matches("^[0-9]+$").WithMessage("Parent number must contain only numbers.")
                .Length(11).WithMessage("Parent number must be exactly 11 digits.");

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
