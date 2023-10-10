using Car_Rental.Common.Classes;
using Car_Rental.Common.Interfaces;
using Car_Rental.Common.Utilities;

namespace Car_Rental.Business.Classes;

public class AddCustomer : BaseService
{
    private string _ssn = string.Empty;

    public AddCustomer(StateManagement stateManagement) : base(stateManagement)
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

    public async Task CreateNewCustomer(int inputGroup)
    {
        _stateManagement.ValidateAggressivelyDict[inputGroup] = true;
        if (string.IsNullOrWhiteSpace(NewCustomer.LastName) || string.IsNullOrWhiteSpace(NewCustomer.FirstName))
        {
            _stateManagement.SetErrorMessage("Please fill in all required fields.");
        }
        else if (ValidSsn != true)
        {
            _stateManagement.SetErrorMessage("Please enter a valid Swedish Personal Identity Number.");
        }
        else if (Ssn.StringCollisionCheck(StateManagement.Customers.Select(person => person.SocialSecurityNumber)))
        {
            _stateManagement.SetErrorMessage("Personal Identity Number already exists.");
            Ssn = string.Empty;
        }
        else
        {
            NewCustomer.SocialSecurityNumber = Ssn;
            AddDataObject(NewCustomer);
            await _stateManagement.RefreshData();
            NewCustomer = new Person("", "", "");
            Ssn = string.Empty;
            ValidSsn = null;
            _stateManagement.ValidateAggressivelyDict[inputGroup] = false;
            _stateManagement.ClearErrorMessage();
        }
    }
}
