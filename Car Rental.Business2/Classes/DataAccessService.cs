using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Common.Utilities;
using Car_Rental.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Car_Rental.Business.Classes;

public class DataAccessService
{
    private readonly IData _dataService;
    public IData DataService => _dataService;

    private readonly DataValidationService _dataValidation;
    public DataValidationService DataValidation => _dataValidation;

    private string _ssn;

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
    public double? OdometerPositionNullable { get; set; }
    public double? CostPerKilometerNullable { get; set; }
    public double? DailyRateNullable { get; set; }
    public IVehicle NewVehicle { get; set; } = new Vehicle("", "");
    public IPerson NewCustomer { get; set; } = new Person("", "", "");
    public List<IBooking> Bookings { get; set; } = new List<IBooking>();
    public List<IPerson> Customers { get; set; } = new List<IPerson>();
    public List<IVehicle> Vehicles { get; set; } = new List<IVehicle>();
    public VehicleType SelectedVehicleType { get; set; }
    public string errorMessage = "errormessage is unchanged";
    public DataAccessService(IData dataService, DataValidationService dataValidation)
    {
        _dataService = dataService;
        _dataValidation = dataValidation;
    }
    public IEnumerable<T> GetDataObjectsOfType<T>() where T : class => _dataService.GetDataObjectsOfType<T>();
    public void AddDataObject(IDataObject dataObject) => _dataService.AddDataObject(dataObject);
    public void RefreshData()
    {
        Bookings = GetDataObjectsOfType<IBooking>().ToList();
        Customers = GetDataObjectsOfType<IPerson>().ToList();
        Vehicles = GetDataObjectsOfType<IVehicle>().ToList();
    }
    public void SelectVehicleType(VehicleType vehicleType)
    {
        SelectedVehicleType = vehicleType;
        NewVehicle.VehicleType = vehicleType;
    }
    public void AddNewVehicle(int inputGroup)
    {
        _dataValidation.FinalCheckDict[inputGroup] = true;
        if (string.IsNullOrWhiteSpace(NewVehicle.RegistrationNumber) ||
            string.IsNullOrWhiteSpace(NewVehicle.Make) ||
            !OdometerPositionNullable.IsPositive() ||
            !CostPerKilometerNullable.IsPositive() ||
            !DailyRateNullable.IsPositive())
        {
            errorMessage = "Please fill in all required fields and ensure numeric values are positive.";
            return;
        }
        NewVehicle.CostPerKilometer = CostPerKilometerNullable ?? 0.0;
        NewVehicle.DailyRate = DailyRateNullable ?? 0.0;
        NewVehicle.OdometerPosition = OdometerPositionNullable ?? 0.0;
        AddDataObject(NewVehicle);
        RefreshData();
        NewVehicle = new Vehicle("", "");
        OdometerPositionNullable = CostPerKilometerNullable = DailyRateNullable = null;
        _dataValidation.FinalCheckDict[inputGroup] = false;
    }
    public void AddNewCustomer(int inputGroup)
    {
        _dataValidation.FinalCheckDict[inputGroup] = true;
        if (string.IsNullOrWhiteSpace(NewCustomer.LastName) || string.IsNullOrWhiteSpace(NewCustomer.FirstName))
        {
            errorMessage = "Please fill in all required fields.";
        }
        else if (ValidSsn != true)
        {
            errorMessage = "Please enter a valid Swedish Personal Identity Number.";
        }
        else
        {
            NewCustomer.SocialSecurityNumber = Ssn;
            AddDataObject(NewCustomer);
            RefreshData();
            NewCustomer = new Person("", "", "");
            Ssn = "";
            ValidSsn = null;
            _dataValidation.FinalCheckDict[inputGroup] = false;
        }
    }
    public void ClearErrorMessage() => errorMessage = "";
}
