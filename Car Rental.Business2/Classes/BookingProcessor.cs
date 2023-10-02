using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Common.Utilities;
using Car_Rental.Data.Classes;
using Car_Rental.Data.Interfaces;

namespace Car_Rental.Business.Classes;
public class BookingProcessor : BaseService
{
    private readonly AddCustomer _addCustomer;
    private readonly AddVehicle _addVehicle;

    public BookingProcessor(DataManagementService dataManagement, AddCustomer addCustomer, AddVehicle addVehicle) : base(dataManagement)
    {
        _addCustomer = addCustomer;
        _addVehicle = addVehicle;
    }

    public Dictionary<IVehicle, IPerson> newBookingsMap = new Dictionary<IVehicle, IPerson>();
    public Dictionary<string, bool> IsRentingMap = new Dictionary<string, bool>();
    public Dictionary<int, int?> DistanceMap = new Dictionary<int, int?>();
    public IPerson DefaultPerson = new Person("", "Select", "person");
    public AddCustomer AddCustomer { get { return _addCustomer; } }
    public AddVehicle AddVehicle { get { return _addVehicle; } }

    public async Task InitializeAsync()
    {
        await _dataManagement.InitializeDataAsync();

        EntityMatcher.MatchAndInsert<IBooking, IVehicle>(
            _dataManagement.DataAccess,
            outerEntity => outerEntity.RegistrationNumber,
            innerEntity => innerEntity.RegistrationNumber,
            (booking, vehicle) => booking.Vehicle = vehicle);
        EntityMatcher.MatchAndInsert<IBooking, IPerson>(
            _dataManagement.DataAccess,
            outerEntity => outerEntity.CustomerSsn,
            innerEntity => innerEntity.SocialSecurityNumber,
            (booking, customer) => booking.Customer = customer);

        SetOdometerPosition();
        CalculateTotalCost();
        RefreshData();

        foreach (var vehicle in _dataManagement.Vehicles)
        {
            newBookingsMap.Add(vehicle, DefaultPerson);
            IsRentingMap.Add(vehicle.RegistrationNumber, false);
        }
    }
    public void RefreshData()
    {
        _dataManagement.RefreshData(); //for debugging purposes
    }
    private void SetOdometerPosition()
    {
        var bookings = _dataManagement.GetDataObjectsOfType<IBooking>();

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

    private void CalculateTotalCost()
    {
        var bookings = _dataManagement.GetDataObjectsOfType<IBooking>();

        foreach (var booking in bookings)
        {
            if (booking.Vehicle != null)
            {
                booking.RentalDays = booking.StartDate.DaysRoundedUp(booking.EndDate);
                decimal totalKilometerCost = booking.Vehicle.CostPerKilometer * ((decimal)booking.OdometerEnd - (decimal)booking.OdometerStart);
                decimal totalDailyCost = booking.Vehicle.DailyRate * booking.RentalDays;
                decimal totalCost = totalKilometerCost + totalDailyCost;

                booking.TotalKilometers = booking.OdometerEnd - booking.OdometerStart;
                booking.TotalCost = totalCost;
                booking.TotalKilometerCost = totalKilometerCost;
                booking.TotalDailyCost = totalDailyCost;
            }
        }
    }

    public void SelectPerson(IVehicle vehicle, IPerson person)
    {
        newBookingsMap[vehicle] = person;
    }

    public void CreateNewBooking (IVehicle vehicle)
    {
        if (newBookingsMap.TryGetValue(vehicle, out var associatedPerson))
        {
            if (associatedPerson.SocialSecurityNumber is "") return;

            var newBooking = new Booking(associatedPerson, vehicle, DateTime.Now);
            newBooking.OdometerStart = newBooking.OdometerEnd = vehicle.OdometerPosition;
            AddDataObject(newBooking);
            RefreshData();
            vehicle.LastBookingId = newBooking.Id;
            DistanceMap.Add(vehicle.LastBookingId, null);
            newBookingsMap[vehicle] = DefaultPerson;
            IsRentingMap[vehicle.RegistrationNumber] = true;
            _dataManagement.Bookings.FirstOrDefault(booking => booking.Id == vehicle.LastBookingId)!.BookingStatus = BookingStatus.Open;
            vehicle.BookingStatus = BookingStatus.Booked;
        }
    }
    public void ReturnVehicle (IVehicle vehicle)
    {
        if (DistanceMap[vehicle.LastBookingId] == null) 
        {
            _dataManagement.SetErrorMessage("Please enter distance driven.");
            return;
        }
        vehicle.OdometerPosition += (double)DistanceMap[vehicle.LastBookingId]!;
        var booking = _dataManagement.Bookings.FirstOrDefault(booking => booking.Id == vehicle.LastBookingId)!;
        booking.EndDate = DateTime.Now;
        booking.OdometerEnd += (double)DistanceMap[vehicle.LastBookingId]!;
        booking.BookingStatus = BookingStatus.Closed;
        booking.Vehicle.BookingStatus = BookingStatus.Available;
        CalculateTotalCost();
        RefreshData();
        IsRentingMap[vehicle.RegistrationNumber] = false;
        _dataManagement.ClearErrorMessage();
    }
}
