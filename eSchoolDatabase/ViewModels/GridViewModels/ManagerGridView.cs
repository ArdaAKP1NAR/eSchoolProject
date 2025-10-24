using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.ViewModels.GridViewModels
{
    public class ManagerGridView
    {
        public long Id { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string IdentityNumber { get; set; } = default!;
    }
}
