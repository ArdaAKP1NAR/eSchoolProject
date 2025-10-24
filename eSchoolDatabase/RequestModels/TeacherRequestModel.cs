using eSchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.RequestModels
{
    public class TeacherRequestModel
    {
        public long Id { get; set; }
        public string IdentityNumber { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public long SchoolId { get; set; }
    }
}
