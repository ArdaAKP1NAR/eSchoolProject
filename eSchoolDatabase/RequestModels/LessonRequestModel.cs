using eSchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.RequestModels
{
    public class LessonRequestModel
    {
        public string Name { get; set; } = default!;
    }
}
