using eSchoolDatabase.Models.Base;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.Models
{
    public class Class : BaseEntity
    {
        [MaxLength(100)]
        public string ClassLevel { get; set; } = default!;
        [MaxLength(100)]
        public string Section { get; set; } = default!;
        public List<Teacher> Teachers { get; set; } = new();
        public List<Student> Students { get; set; } = new();
        public List<Lesson> Lessons { get; set; } = new();
        public School School { get; set; } = default!;
        public long SchoolId { get; set; }
    }
}
