using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.Models
{
    public class Manager : BaseEntity
    {
        [MaxLength(100)]
        public string IdentityNumber { get; set; } = default!;
        [MaxLength(100)]
        public string Password { get; set; } = default!;
        [MaxLength(100)]
        public string Name { get; set; } = default!;
        public School School { get; set; } = default!;
        public long SchoolId { get; set; }
    }
}
