using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.ViewModels.GridViewModels
{
    public class StudentGridView
    {
        public long Id { get; set; }
        public string IdentityNumber { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int StudentNumber { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public AddressViewModel Address { get; set; } = default!;
        public string ParentNumber { get; set; } = default!;
    }
}
