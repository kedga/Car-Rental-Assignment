using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;

namespace Car_Rental.Business.Classes;
public class BookingProcessor
{
    public event Action DataChanged = delegate { };

    private string _errorMessage = string.Empty;

    private readonly IData _data;
    public IData Data => _data;

    private readonly AddCustomer _addCustomer;
    private readonly AddVehicle _addVehicle;
    private readonly AddBooking _addBooking;
    private readonly VehicleSorter _vehicleSorter;
    public BookingProcessor(IData data)
    {
        _data = data;
        _addCustomer = new AddCustomer(this);
        _addVehicle = new AddVehicle(this);
        _addBooking = new AddBooking(this);
        _vehicleSorter = new VehicleSorter(this, data);
    }
    public AddCustomer AddCustomer => _addCustomer;
    public AddVehicle AddVehicle => _addVehicle;
    public AddBooking AddBooking => _addBooking;
    public VehicleSorter VehicleSorter => _vehicleSorter;
    public Dictionary<int, bool> ValidateAggressivelyDict { get; set; } = Enumerable.Range(1, 10).ToDictionary(x => x, x => false);
    public List<IVehicle> Vehicles => _data.Get<IVehicle>(v => true);
    public List<IPerson> Customers => _data.Get<IPerson>(p => true);
    public List<IBooking> Bookings => _data.Get<IBooking>(b => true);
    public void AddDataObject(IDataObject dataObject) => _data.Add(dataObject);

    public async Task InitializeAsync()
    {
        await _data.Initialize();

        RefreshData();

        InitializeOdometerPositions(Bookings);

        foreach (var booking in Bookings)
        {
            if (booking.Vehicle != null)
            {
                booking.CalculateTotalCost();
            }
        }

        RefreshData();

        foreach (var vehicle in Vehicles)
        {
            _addBooking.VehicleToPersonBookingsMap.Add(vehicle, _addBooking.DefaultPerson);
            _addBooking.IsVehicleCurrentlyRentedMap.Add(vehicle, false);
        }
    }
    public void RefreshData()
    {
        DataChanged?.Invoke();
    }
    private void InitializeOdometerPositions(IEnumerable<IBooking> bookings)
    {
        Console.WriteLine("Initializing Odometer Positions...");

        var groupedBookings = bookings.GroupBy(booking => booking.Vehicle);

        foreach (var group in groupedBookings)
        {
            Console.WriteLine($"Processing Vehicle: {group.Key?.RegistrationNumber}");

            var latestBooking = group.OrderByDescending(booking => booking.EndDate).FirstOrDefault();

            if (latestBooking != null)
            {
                Console.WriteLine($"Setting Odometer Position for Vehicle {latestBooking.Vehicle.RegistrationNumber} to {latestBooking.OdometerEnd}");

                latestBooking.Vehicle.OdometerPosition = latestBooking.OdometerEnd;
            }
            else
            {
                Console.WriteLine($"No bookings found for Vehicle {group.Key?.RegistrationNumber}");
            }
        }
        Console.WriteLine("Odometer Position Initialization Complete.");
    }
    public void SetErrorMessage(string message)
    {
        _errorMessage = message;
        DataChanged?.Invoke();
    }
    public string GetErrorMessage() => _errorMessage;
    public void ClearErrorMessage()
    {
        _errorMessage = string.Empty;

        foreach (var key in ValidateAggressivelyDict.Keys.ToList())
        {
            ValidateAggressivelyDict[key] = false;
        }

        DataChanged?.Invoke();
    }

}
