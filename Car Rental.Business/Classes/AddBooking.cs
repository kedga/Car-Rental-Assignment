using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Common.Utilities;

namespace Car_Rental.Business.Classes;

public class AddBooking
{
    private readonly BookingProcessor _bp;

    public Dictionary<IVehicle, IPerson> VehicleToPersonBookingsMap { get; private set; } = new();
    public Dictionary<IVehicle, bool> IsVehicleCurrentlyRentedMap { get; private set; } = new();
    public Dictionary<int, int?> DistanceMap { get; private set; } = new();
    public IPerson DefaultPerson { get; private set; } = new Person("", "Select", "person");
    public bool IsProcessing { get; private set; }
    public bool UseDelay { get; set; } = true;
    public AddBooking(BookingProcessor bp)
    {
        _bp = bp;
    }
    public void SelectPerson(IVehicle vehicle, IPerson person)
    {
        VehicleToPersonBookingsMap[vehicle] = person;
    }
    public async Task CreateNewBooking(IVehicle vehicle)
    {
        if (!VehicleToPersonBookingsMap.TryGetValue(vehicle, out var associatedPerson)) return;

        if (associatedPerson.SocialSecurityNumber is "") return;

        _bp.ClearErrorMessage();

        if (UseDelay)
        {
            IsProcessing = true;
            _bp.RefreshData();
            await Task.Delay(10000);
            IsProcessing = false;
        }

        var newBooking = new Booking(associatedPerson, vehicle, DateTime.Now);
        newBooking.OdometerStart = newBooking.OdometerEnd = vehicle.OdometerPosition;

        newBooking.Id = _bp.Bookings.Select(x => x.Id).Max() + 1;

        await _bp.Data.Add(newBooking);

        vehicle.LastBookingId = newBooking.Id;
        DistanceMap.Add(vehicle.LastBookingId, null);
        VehicleToPersonBookingsMap[vehicle] = DefaultPerson;
        IsVehicleCurrentlyRentedMap[vehicle] = true;
        var lastBooked = _bp.Data.Single<IBooking>(b => b.Id == vehicle.LastBookingId);
        if (lastBooked is null) throw new InvalidOperationException("lastBooked is null in CreateNewBooking.");
        lastBooked.BookingStatus = BookingStatus.Open;
        vehicle.BookingStatus = BookingStatus.Booked;
    }
    public void ReturnVehicle(IVehicle vehicle)
    {
        if (DistanceMap[vehicle.LastBookingId] == null)
        {
            _bp.SetErrorMessage("Please enter distance driven.");
            return;
        }

        vehicle.OdometerPosition += (double)DistanceMap[vehicle.LastBookingId]!;
        var booking = _bp.Data.Single<IBooking>(b => b.Id == vehicle.LastBookingId);
        if (booking is null) throw new InvalidOperationException("booking is null in ReturnVehicle.");
        booking.EndDate = DateTime.Now;
        booking.OdometerEnd += (double)DistanceMap[vehicle.LastBookingId]!;
        booking.BookingStatus = BookingStatus.Closed;
        booking.Vehicle.BookingStatus = BookingStatus.Available;
        booking.CalculateTotalCost();
		_bp.RefreshData();
        IsVehicleCurrentlyRentedMap[vehicle] = false;
        _bp.ClearErrorMessage();
    }
	public void ToggleUseDelay()
    {
        if (IsProcessing is true) return;
        UseDelay = !UseDelay;
    }
}
