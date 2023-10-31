using Car_Rental.Common.Classes;
using Car_Rental.Common.Interfaces;
using Car_Rental.Common.Utilities;

namespace Car_Rental.Business.Classes;

public class AddCustomer
{
    private readonly BookingProcessor _bp;
    private string _ssn = string.Empty;

    public AddCustomer(BookingProcessor bp)
    {
        _bp = bp;
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
        _bp.ValidateAggressivelyDict[inputGroup] = true;
        if (string.IsNullOrWhiteSpace(NewCustomer.LastName) || string.IsNullOrWhiteSpace(NewCustomer.FirstName))
        {
            _bp.SetErrorMessage("Please fill in all required fields.");
        }
        else if (ValidSsn != true)
        {
            _bp.SetErrorMessage("Please enter a valid Swedish Personal Identity Number.");
        }
        else if (Ssn.StringCollisionCheck(_bp.Customers.Select(person => person.SocialSecurityNumber)))
        {
            _bp.SetErrorMessage("Personal Identity Number already exists.");
            Ssn = string.Empty;
        }
        else
        {
            NewCustomer.SocialSecurityNumber = Ssn;
            _bp.AddDataObject(NewCustomer);
            _bp.RefreshData();
            NewCustomer = new Person("", "", "");
            Ssn = string.Empty;
            ValidSsn = null;
            _bp.ValidateAggressivelyDict[inputGroup] = false;
            _bp.ClearErrorMessage();
        }
    }
}
