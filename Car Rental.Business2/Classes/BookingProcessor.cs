using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Classes;
using Car_Rental.Data.Interfaces;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Car_Rental.Common.Utilities;

namespace Car_Rental.Business.Classes;
public class BookingProcessor
{
    private readonly DataValidationService _dataValidation;
    private readonly DataAccessService _dataAccess;
    public BookingProcessor(DataValidationService dataValidation, DataAccessService dataAccess)
    {
        _dataValidation = dataValidation;
        _dataAccess = dataAccess;
    }
    public DataAccessService DataAccess => _dataAccess;
    public DataValidationService DataValidation => _dataValidation;
    public async Task InitializeAsync()
    {
        await Console.Out.WriteLineAsync("OnInitializedAsync running");
        await DataAccess.DataService.InitializeDataAsync();
        MatchBookingsWithVehicles();
        MatchBookingsWithCustomers();
        SetOdometerPosition();
        CalculateTotalCost();
        _dataAccess.RefreshData();
    }

    private void MatchBookingsWithVehicles()
    {
        var bookings = DataAccess.DataService.GetDataObjectsOfType<IBooking>();
        var vehicles = DataAccess.DataService.GetDataObjectsOfType<IVehicle>();

        var joinedBookings = from booking in bookings
                             join vehicle in vehicles on booking.RegistrationNumber equals vehicle.RegistrationNumber
                             select new { Booking = booking, Vehicle = vehicle };

        foreach (var item in joinedBookings)
        {
            item.Booking.Vehicle = item.Vehicle;
        }
    }
    private void MatchBookingsWithCustomers()
    {
        var bookings = DataAccess.DataService.GetDataObjectsOfType<IBooking>();
        var people = DataAccess.DataService.GetDataObjectsOfType<IPerson>();

        var joinedBookings = from booking in bookings
                             join customer in people on booking.CustomerSsn equals customer.SocialSecurityNumber
                             select new { Booking = booking, Person = customer };

        foreach (var item in joinedBookings)
        {
            item.Booking.Customer = item.Person;
        }
    }

    private void SetOdometerPosition()
    {
        var bookings = DataAccess.DataService.GetDataObjectsOfType<IBooking>();

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
        var bookings = DataAccess.DataService.GetDataObjectsOfType<IBooking>();

        foreach (var booking in bookings)
        {
            if (booking.Vehicle != null)
            {
                int rentalDays = booking.EndDate.Duration(booking.StartDate);
                double totalKilometerCost = booking.Vehicle.CostPerKilometer * (booking.OdometerEnd - booking.OdometerStart);
                double totalDailyCost = booking.Vehicle.DailyRate * rentalDays;
                double totalCost = totalKilometerCost + totalDailyCost;

                booking.RentalDays = rentalDays;
                booking.TotalKilometers = booking.OdometerEnd - booking.OdometerStart;
                booking.TotalCost = totalCost;
                booking.TotalKilometerCost = totalKilometerCost;
                booking.TotalDailyCost = totalDailyCost;
            }
        }
    }
}
