using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.RequestModels
{
    public class ClassRequestModel
    {
        public string ClassLevel { get; set; } = default!;
        public string Section { get; set; } = default!;
        public long SchoolId { get; set; }
    }
}
