using eSchoolDatabase.Models;
using eSchoolDatabase.RequestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.RequestModels
{
    public class StudentRequestModel
    {
        public StudentRequestModel()
        {
            Address = new();
        }
        public long Id { get; set; }
        public string IdentityNumber { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int StudentNumber { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public AddressRequestModel Address { get; set; } = default!;
        public string ParentNumber { get; set; } = default!;
        public long SchoolId { get; set; }
    }
}
