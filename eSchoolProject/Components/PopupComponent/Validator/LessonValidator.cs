using eSchoolDatabase.RequestModels;
using FluentValidation;

namespace eSchoolProject.Components.PopupComponent.Validator
{
    public class LessonValidator : AbstractValidator<LessonRequestModel>
    {
        public LessonValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");
            RuleFor(x => x.CourseCode)
                .NotEmpty()
                .WithMessage("Course code is required.")
                .MaximumLength(50)
                .WithMessage("Course code must not exceed 50 characters.");
            RuleFor(x => x.TeacherId)
                .GreaterThan(0)
                .WithMessage("Teacher ID must be a positive number.");
            RuleFor(x => x.ClassList)
                .NotEmpty()
                .WithMessage("At least one class must be selected.");
        }
    }
}
