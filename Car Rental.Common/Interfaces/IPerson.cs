using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Interfaces;

public interface IPerson
{
    string SocialSecurityNumber { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
}
