using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
