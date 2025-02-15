using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.ViewModels
{
    public class AddressViewModel
    {
        public long Id { get; set; }
        public string City { get; set; } = default!;
        public string District { get; set; } = default!;
        public string Street { get; set; } = default!;
    }
}
