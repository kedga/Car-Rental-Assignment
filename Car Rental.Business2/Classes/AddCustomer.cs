using Car_Rental.Common.Classes;
using Car_Rental.Common.Interfaces;
using Car_Rental.Common.Utilities;
using Car_Rental.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Business.Classes;

public class AddCustomer : BaseService
{

    private string _ssn = string.Empty;

    public AddCustomer(DataManagementService dataManagement) : base(dataManagement)
    {
    }

    public string Ssn
    {
        get { return _ssn; }
        set
        {
            var (formattedSsn, valid) = value.FormatSsn();
            _ssn = formattedSsn;
            ValidSsn = valid;
        }
    }
    public bool? ValidSsn { get; set; }
    public bool? ValidFirstName { get; set; }
    public bool? ValidLastName { get; set; }
    public IPerson NewCustomer { get; set; } = new Person("", "", "");

    public void CreateNewCustomer(int inputGroup)
    {
        _dataManagement.ValidateAggressivelyDict[inputGroup] = true;
        if (string.IsNullOrWhiteSpace(NewCustomer.LastName) || string.IsNullOrWhiteSpace(NewCustomer.FirstName))
        {
            _dataManagement.errorMessage = "Please fill in all required fields.";
        }
        else if (ValidSsn != true)
        {
            _dataManagement.errorMessage = "Please enter a valid Swedish Personal Identity Number.";
        }
        else if (Ssn.StringCollisionCheck(DataManagement.Customers.Select(person => person.SocialSecurityNumber)))
        {
            _dataManagement.errorMessage = "Personal Identity Number already exists.";
            Ssn = string.Empty;
        }
        else
        {
            NewCustomer.SocialSecurityNumber = Ssn;
            AddDataObject(NewCustomer);
            _dataManagement.RefreshData();
            NewCustomer = new Person("", "", "");
            Ssn = string.Empty;
            ValidSsn = null;
            _dataManagement.ValidateAggressivelyDict[inputGroup] = false;
            _dataManagement.ClearErrorMessage();
        }
    }
}
