using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.RequestModels
{
    public class ClassRequestModel
    {
        public string ClassName { get; set; } = default!;
        public long SchoolId { get; set; }
    }
}
