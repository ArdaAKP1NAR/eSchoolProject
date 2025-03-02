using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.Models
{
    public class Lesson : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = default!;
        public long ClassId { get; set; } // düşün burayı student olması lazım sanki
        public Class Class { get; set; } = default!;
        public List<Student> Students { get; set; } = new();
        public long TeacherId { get; set; }
        public Teacher Teacher { get; set; } = default!;
    }

}
