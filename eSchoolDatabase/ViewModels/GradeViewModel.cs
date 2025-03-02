using eSchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.ViewModels
{
    public class GradeViewModel
    {
        public long Id { get; set; }
        public double GradeValue { get; set; }
        public long LessonId { get; set; }
        public long StudentId { get; set; }
    }
}
