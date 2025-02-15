using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.Models
{
    public class Address : BaseEntity
    {
        [MaxLength(100)]
        public string City { get; set; } = default!;
        [MaxLength(100)]
        public string District { get; set; } = default!;
        [MaxLength(100)]
        public string Street { get; set; } = default!;
    }
}
