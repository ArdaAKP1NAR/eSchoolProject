using eSchoolDatabase.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace eSchoolDatabase.Models
{
    public class User : BaseEntity
    {
        [MaxLength(100)]
        public string IdentityNumber { get; set; } = default!;
        [MaxLength(100)]
        public string Password { get; set; } = default!;
        public List<Roles> Roles { get; set; } = new(); 
    }
}
