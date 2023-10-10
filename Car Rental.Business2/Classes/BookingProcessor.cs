using Car_Rental.Common.Interfaces;
using Car_Rental.Common.Utilities;
using Car_Rental.Data.Classes;

namespace Car_Rental.Business.Classes;
public class BookingProcessor : BaseService
{
    private readonly AddCustomer _addCustomer;
    private readonly AddVehicle _addVehicle;
    private readonly AddBooking _addBooking;
    private readonly VehicleSorter _vehicleSorter;

    public BookingProcessor(StateManagement stateManagement, AddCustomer addCustomer, AddVehicle addVehicle, AddBooking addBooking, VehicleSorter vehicleSorter) : base(stateManagement)
    {
        _addCustomer = addCustomer;
        _addVehicle = addVehicle;
        _addBooking = addBooking;
        _vehicleSorter = vehicleSorter;
    }

    public AddCustomer AddCustomer => _addCustomer;
    public AddVehicle AddVehicle => _addVehicle;
    public AddBooking AddBooking => _addBooking;
    public VehicleSorter VehicleSorter => _vehicleSorter;

    public async Task InitializeAsync()
    {
        await _stateManagement.InitializeDataAsync();

        InitializeOdometerPositions(_stateManagement.Bookings);

        foreach (var booking in _stateManagement.Bookings)
        {
            if (booking.Vehicle != null)
            {
                booking.CalculateTotalCost();
            }
        }

        await _stateManagement.RefreshData();

        foreach (var vehicle in _stateManagement.Vehicles)
        {
            _addBooking.VehicleToPersonBookingsMap.Add(vehicle, _addBooking.DefaultPerson);
            _addBooking.IsVehicleCurrentlyRentedMap.Add(vehicle, false);
        }
    }
    private void InitializeOdometerPositions(IEnumerable<IBooking> bookings)
    {
        var groupedBookings = bookings.GroupBy(booking => booking.Vehicle);

        foreach (var group in groupedBookings)
        {
            var latestBooking = group.OrderByDescending(booking => booking.EndDate).FirstOrDefault();

            if (latestBooking != null)
            {
                latestBooking.Vehicle.OdometerPosition = latestBooking.OdometerEnd;
            }
        }
    }
    
}
