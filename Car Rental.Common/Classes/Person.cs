using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Car_Rental.Common.Interfaces;

namespace Car_Rental.Common.Classes;

public class Person : IPerson
{
	public Person(string socialSecurityNumber, string firstName, string lastName)
	{
		SocialSecurityNumber = socialSecurityNumber;
		FirstName = firstName;
		LastName = lastName;
	}

	public string SocialSecurityNumber { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
}
