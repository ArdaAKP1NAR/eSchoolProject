using AutoMapper;
using AutoMapper.QueryableExtensions;
using eSchoolDatabase;
using eSchoolDatabase.Models;
using eSchoolDatabase.Repositories;
using eSchoolDatabase.Repositories.Interface;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Exceptions;
using eSchoolProject.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using static MudBlazor.Colors;

namespace eSchoolProject.Services
{
    public class GradeService(IMapper mapper, IStudentService studentService, IGradeRepository gradeRepository, ITransactionService transactionService, IStudentRepository studentRepository) : IGradeService
    {
        public async Task SaveGradesAsync(List<GradeInputModel> grades, CancellationToken cancellationToken)
        {
            using var transaction = transactionService.BeginTransaction();

            try
            {
                foreach (var gradeInputModel in grades)
                {
                    var existingGrade = await gradeRepository.GetAll()
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
                }

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new SaveGradeException("An error occurred while saving grades.");
            }
        }

        public async Task<List<StudentViewModel>> GetStudentsByClassAndTeacherAsync(long classId, long teacherId, CancellationToken cancellationToken)
        {
            return await studentRepository.GetAll()
                .Where(s => s.ClassId == classId
                            && s.Class.Lessons.Any(l => l.TeacherId == teacherId))
                .ProjectTo<StudentViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<GradeViewModel>> GetGradesByLessonAndStudentAsync(long lessonId, List<long> studentIds, CancellationToken cancellationToken)
        {
            return await gradeRepository.GetAll()
                .Where(g => g.LessonId == lessonId && studentIds.Contains(g.StudentId))
                .ProjectTo<GradeViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<StudentViewModel>> LoadGradesForSelectedLesson(long classId, long lessonId, long teacherId, CancellationToken cancellationToken)
        {
            try
            {
                return await studentRepository.GetAll()
                  .Include(a=> a.Grades)
                  .Where(s => s.ClassId == classId
                              && s.Class.Lessons.Any(l => l.TeacherId == teacherId))
                  .Select(student => new StudentViewModel()
                  {
                      Id = student.Id,
                      Name = student.Name,
                      StudentNumber = student.StudentNumber,
                      IdentityNumber = student.IdentityNumber,
                      Midterm = student.Grades.Where(a=> a.LessonId == lessonId && a.GradeType == GradeType.Midterm).Select(a=> a.GradeValue).FirstOrDefault(),
                      Final = student.Grades.Where(a => a.LessonId == lessonId && a.GradeType == GradeType.Final).Select(a => a.GradeValue).FirstOrDefault(),
                      Oral = student.Grades.Where(a => a.LessonId == lessonId && a.GradeType == GradeType.Oral).Select(a => a.GradeValue).FirstOrDefault(),
                      Homework = student.Grades.Where(a => a.LessonId == lessonId && a.GradeType == GradeType.Homework).Select(a => a.GradeValue).FirstOrDefault()
                  }).ToListAsync(cancellationToken);
            }
            catch
            {
                throw new LoadGradesException("Öğrenci notları yüklenirken hata oluştu.");
            }
        }
    }
}
