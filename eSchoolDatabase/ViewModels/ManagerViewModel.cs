using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.ViewModels
{
    public class ManagerViewModel
    {
        public long Id { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public string IdentityNumber { get; set; } = default!;
        public string Name { get; set; } = default!;
        public long SchoolId { get; set; }
    }
}
