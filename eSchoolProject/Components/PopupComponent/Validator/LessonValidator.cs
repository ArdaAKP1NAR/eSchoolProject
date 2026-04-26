using eSchoolDatabase.Repositories.Interface;
using eSchoolDatabase.RequestModels;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace eSchoolProject.Components.PopupComponent.Validator
{
    public class LessonValidator : AbstractValidator<LessonRequestModel>
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public LessonValidator(IServiceScopeFactory serviceScopeFactory)
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
                .WithMessage("Course code must not exceed 50 characters.")
                .MustAsync(async (obj ,_,_, cancellationToken) => await CourseMustNotExist(obj,cancellationToken))
                .WithMessage("Course already exists.")
                ;
            RuleFor(x => x.Teacher)
                .NotNull()
                .WithMessage("Teacher must be selected.");
            RuleFor(x => x.ClassList)
                .NotEmpty()
                .WithMessage("At least one class must be selected.");
            this.serviceScopeFactory = serviceScopeFactory;
        }
        private async Task<bool> CourseMustNotExist(LessonRequestModel q,CancellationToken cancellationToken)
        {
            using var scope = serviceScopeFactory.CreateScope();
            return !await scope.ServiceProvider.GetRequiredService<ILessonRepository>().GetAll().AnyAsync(a => a.CourseCode == q.CourseCode,cancellationToken );
        }
    }
}
