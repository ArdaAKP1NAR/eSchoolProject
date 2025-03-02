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
        public string ClassName { get; set; } = default!;
        public List<Teacher>? Teachers { get; set; } = default!;
        public List<Student>? Students { get; set; } = default!;
        public School School { get; set; } = default!;
        public long SchoolId { get; set; }
    }
}
