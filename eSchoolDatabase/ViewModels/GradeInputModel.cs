using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.ViewModels
{
    public class GradeInputModel
    {
        public long Id { get; set; }
        public long StudentId { get; set; }
        public long LessonId { get; set; }
        public double? Grade{ get; set; } = new();
        public GradeType GradeType { get; set; } // enum eklenmeli
    }
}
