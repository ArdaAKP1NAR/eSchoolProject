using eSchoolDatabase.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace eSchoolDatabase.Models
{
    public class School : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = default!;
        public Address Address { get; set; } = default!;
        public long AddressId { get; set; }
        public List<Manager> Managers { get; set; } = new();
        public List<Teacher> Teachers { get; set; } = new();
        public List<Student> Students { get; set; } = new();
        public List<Class> Classes { get; set; } = new();
    }
}
