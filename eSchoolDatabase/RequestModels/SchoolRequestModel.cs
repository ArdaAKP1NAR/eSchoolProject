using eSchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.RequestModel
{
    public class SchoolRequestModel
    {
        public string Name { get; set; } = default!;
        public AddressRequestModel Adress { get; set; } = default!;

        public static SchoolRequestModel New()
        {
            return new()
            {
                Adress = new()
            };
        }
    }
}
