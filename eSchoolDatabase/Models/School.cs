using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.Models
{
    public class School : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = default!;
        public Address Address { get; set; } = default!;
        public long AddressId { get; set; }
        public List<Manager>? Managers { get; set; } = default!;
        public List<Teacher>? Teachers { get; set; } = default!;
        public List<Student>? Students { get; set; } = default!;
        public List<Class>? Classes { get; set; } = default!;
    }
}
