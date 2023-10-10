using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Common.Utilities;

namespace Car_Rental.Business.Classes;

public class AddBooking : BaseService
{
    public Dictionary<IVehicle, IPerson> VehicleToPersonBookingsMap { get; private set; } = new();
    public Dictionary<IVehicle, bool> IsVehicleCurrentlyRentedMap { get; private set; } = new();
    public Dictionary<int, int?> DistanceMap { get; private set; } = new();
    public IPerson DefaultPerson { get; private set; } = new Person("", "Select", "person");
    public bool IsProcessing { get; private set; }
    public bool UseDelay { get; set; } = true;
    public AddBooking(StateManagement stateManagement) : base(stateManagement)
    {
    }
    public void SelectPerson(IVehicle vehicle, IPerson person)
    {
        VehicleToPersonBookingsMap[vehicle] = person;
    }
    public async Task CreateNewBooking(IVehicle vehicle)
    {
        if (!VehicleToPersonBookingsMap.TryGetValue(vehicle, out var associatedPerson)) return;

        if (associatedPerson.SocialSecurityNumber is "") return;

        _stateManagement.ClearErrorMessage();

        if (UseDelay)
        {
            IsProcessing = true;
            await _stateManagement.RefreshData();
            await Task.Delay(10000);
            IsProcessing = false;
        }

        var newBooking = new Booking(associatedPerson, vehicle, DateTime.Now);
        newBooking.OdometerStart = newBooking.OdometerEnd = vehicle.OdometerPosition;
        AddDataObject(newBooking);

        await _stateManagement.RefreshData(); // add new booking to Bookings and give it an Id

        vehicle.LastBookingId = newBooking.Id;
        DistanceMap.Add(vehicle.LastBookingId, null);
        VehicleToPersonBookingsMap[vehicle] = DefaultPerson;
        IsVehicleCurrentlyRentedMap[vehicle] = true;
        _stateManagement.Bookings.First(booking => booking.Id == vehicle.LastBookingId).BookingStatus = BookingStatus.Open;
        vehicle.BookingStatus = BookingStatus.Booked;
    }
    public async Task ReturnVehicle(IVehicle vehicle)
    {
        if (DistanceMap[vehicle.LastBookingId] == null)
        {
            _stateManagement.SetErrorMessage("Please enter distance driven.");
            return;
        }

        vehicle.OdometerPosition += (double)DistanceMap[vehicle.LastBookingId]!;
        var booking = _stateManagement.Bookings.FirstOrDefault(booking => booking.Id == vehicle.LastBookingId)!;
        booking.EndDate = DateTime.Now;
        booking.OdometerEnd += (double)DistanceMap[vehicle.LastBookingId]!;
        booking.BookingStatus = BookingStatus.Closed;
        booking.Vehicle.BookingStatus = BookingStatus.Available;
        booking.CalculateTotalCost();
		await _stateManagement.RefreshData();
        IsVehicleCurrentlyRentedMap[vehicle] = false;
        _stateManagement.ClearErrorMessage();
    }
	public void ToggleUseDelay()
    {
        if (IsProcessing is true) return;
        UseDelay = !UseDelay;
    }
}
