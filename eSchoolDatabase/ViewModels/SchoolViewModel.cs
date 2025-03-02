using eSchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.ViewModels
{
    public class SchoolViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = default!;
        public AddressViewModel Address { get; set; } = default!;
        public List<ManagerViewModel>? Managers { get; set; }
        public List<TeacherViewModel>? Teachers { get; set; }
        public List<StudentViewModel>? Students { get; set; }
        public List<ClassViewModel>? Classes { get; set; } 
    }
}
