using AutoMapper;
using eSchoolDatabase.Models;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolDatabase.Repositories.Interface;
using eSchoolProject.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eSchoolDatabase.Repositories;
using eSchoolProject.Exceptions;
using static MudBlazor.CategoryTypes;
using static MudBlazor.Colors;
using System.Threading;

namespace eSchoolProject.Services
{
    public class AttendanceService(ILessonScheduleRepository _lessonScheduleRepository, IAttendanceRepository _attendanceRepository, IStudentRepository _studentRepository, IMapper _mapper, ITransactionService _transactionService) : IAttendanceService
    {
        public async Task<List<ClassViewModel>> GetTeacherClassesAsync(long teacherId)
        {
            var teacherClasses = await _lessonScheduleRepository.GetAll()
                .Where(ls => ls.TeacherId == teacherId)
                .Include(ls => ls.Class)
                .Select(ls => ls.Class)
                .Distinct()
                .ToListAsync();

            return _mapper.Map<List<ClassViewModel>>(teacherClasses);
        }

        public async Task<List<LessonScheduleViewModel>> GetLessonSchedulesAsync(long teacherId, long classId, DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;

            var schedules = await _lessonScheduleRepository.GetAll()
                .Include(ls => ls.Class)
                .Include(ls => ls.Lesson)
                .Include(ls => ls.Teacher)
                .Where(ls => ls.TeacherId == teacherId 
                        && ls.ClassId == classId 
                        && ls.Day == dayOfWeek)
                .OrderBy(ls => ls.StartTime)
                .ToListAsync();

            return _mapper.Map<List<LessonScheduleViewModel>>(schedules);
        }

        public async Task<List<AttendanceViewModel>> GetStudentAttendanceListAsync(long lessonScheduleId, DateTime date)
        {
            var schedule = await _lessonScheduleRepository.GetAll()
                .Include(ls => ls.Class)
                .FirstOrDefaultAsync(ls => ls.Id == lessonScheduleId);

            if (schedule == null) return new List<AttendanceViewModel>();

            var students = await _studentRepository.GetAll()
                .Where(s => s.ClassId == schedule.ClassId)
                .OrderBy(s => s.StudentNumber)
                .ToListAsync();

            if (!students.Any()) return new List<AttendanceViewModel>();

            var studentIds = students.Select(s => s.Id).ToList();

            var existingAttendances = await _attendanceRepository.GetAll()
                .Where(a => a.LessonId == schedule.LessonId 
                         && a.AttendanceDate.Date == date.Date
                         && a.StudentId.HasValue
                         && studentIds.Contains(a.StudentId.Value))
                .ToListAsync();

            var viewModels = new List<AttendanceViewModel>();

            foreach (var student in students)
            {
                var attendance = existingAttendances.FirstOrDefault(a => a.StudentId == student.Id);
                
                AttendanceViewModel vm;
                if (attendance != null)
                {
                    vm = _mapper.Map<AttendanceViewModel>(attendance);
                    vm.IsRecorded = true;
                }
                else
                {
                    vm = _mapper.Map<AttendanceViewModel>(student);
                    vm.IsRecorded = false;
                    vm.IsPresent = true; 
                }

                vm.ClassId = schedule.ClassId;
                vm.LessonId = schedule.LessonId;
                vm.TeacherId = schedule.TeacherId;
                vm.AttendanceDate = date;

                viewModels.Add(vm);
            }

            return viewModels;
        }

        public async Task<bool> SaveAttendanceAsync(List<AttendanceRequestModel> models, CancellationToken cancellationToken)
        {
            if (models == null || !models.Any()) return false;

            // 1. Bulk Read existing records to avoid N+1 Selects
            var firstModel = models.First();
            var studentIds = models.Select(m => m.StudentId).Distinct().ToList();
            var lessonId = firstModel.LessonId;
            var date = firstModel.AttendanceDate.Date;

            // Assuming all models belong to same Lesson/Date batch. 
            // If mixed, query needs to be broader, but usually UI sends one class list.
            var existingAttendances = await _attendanceRepository.GetAll()
                .Where(a => a.LessonId == lessonId 
                         && a.AttendanceDate.Date == date 
                         && studentIds.Contains(a.StudentId.Value))
                .ToListAsync(cancellationToken);

            var toAdd = new List<Attendance>();
            var toUpdate = new List<Attendance>();

            foreach (var model in models)
            {
                var existing = existingAttendances.FirstOrDefault(a => a.StudentId == model.StudentId);
                if (existing != null)
                {
                    _mapper.Map(model, existing); // Updates 'existing' entity tracked by EF
                    toUpdate.Add(existing);
                }
                else
                {
                    var newEntity = _mapper.Map<Attendance>(model);
                    toAdd.Add(newEntity);
                }
            }

            // 2. Transaction for Atomicity
            using var transaction = _transactionService.BeginTransaction();
            try
            {
                if (toUpdate.Any())
                {
                    await _attendanceRepository.UpdateRangeAsync(toUpdate, cancellationToken);
                }

                if (toAdd.Any())
                {
                    await _attendanceRepository.AddRangeAsync(toAdd, cancellationToken);
                }

                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw; // Better to rethrow or custom exception
            }
        }
    }
}
