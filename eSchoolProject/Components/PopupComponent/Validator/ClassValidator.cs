using eSchoolDatabase.RequestModels;
using FluentValidation;

namespace eSchoolProject.Components.PopupComponent.Validator
{
    public class ClassValidator : AbstractValidator<ClassRequestModel>
    {
        public ClassValidator()
        {
            RuleFor(a => a.ClassName)
                .NotNull().WithMessage("Class name is required")
                .NotEmpty().WithMessage("Class name cannot be empty");
        }
    }
}
