using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.ViewModels.GridViewModels
{
    public class ClassGridView
    {
        public long Id { get; set; }
        public string ClassLevel { get; set; } = default!;
        public string Section { get; set; } = default!;
        public string FullClassName => $"{ClassLevel}{Section}";
        public long SchoolId { get; set; }
    }
}
