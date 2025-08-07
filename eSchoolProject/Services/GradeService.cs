using AutoMapper;
using eSchoolDatabase.Models;
using eSchoolDatabase.Repositories.Interface;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace eSchoolProject.Services
{
    public class GradeService(IMapper mapper, IGradeRepository gradeRepository) : IGradeService
    {
        public async Task AddOrUpdateGradeAsync(GradeInputModel gradeInputModel, CancellationToken cancellationToken)
        {
            var existingGrade = await gradeRepository.GetAll(cancellationToken)
                .FirstOrDefaultAsync(g =>
                    g.StudentId == gradeInputModel.StudentId &&
                    g.LessonId == gradeInputModel.LessonId &&
                    g.GradeType == gradeInputModel.GradeType,
                    cancellationToken);

            if (existingGrade != null)
            {
                existingGrade.GradeValue = gradeInputModel.Grade ?? existingGrade.GradeValue;
                await gradeRepository.UpdateAsync(existingGrade);
            }
            else
            {
                var newGrade = mapper.Map<Grade>(gradeInputModel);
                await gradeRepository.AddAsync(newGrade, cancellationToken);
            }

            await gradeRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
