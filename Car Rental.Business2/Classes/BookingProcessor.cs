using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Classes;
using Car_Rental.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Business.Classes;
public class BookingProcessor
{
    public readonly IData _dataService;
    public BookingProcessor(IData dataService) => _dataService = dataService;

    
    public async Task InitializeDataAsync()
    {
        await _dataService.InitializeDataAsync();
        MatchBookingsWithVehicles();
        SetOdometerPosition();
        CalculateTotalCost();
    }
    public IEnumerable<IBooking> GetBookings() => _dataService.GetDataObjectsOfType<IBooking>();
    public IEnumerable<IVehicle> GetVehicles() => _dataService.GetDataObjectsOfType<IVehicle>();
    public IEnumerable<IPerson> GetCustomers() => _dataService.GetDataObjectsOfType<IPerson>();
    public void AddDataObject(IDataObject dataObject) => _dataService.AddDataObject(dataObject);

    private void MatchBookingsWithVehicles()
    {
        var bookings = _dataService.GetDataObjectsOfType<IBooking>();
        var vehicles = _dataService.GetDataObjectsOfType<IVehicle>();

        var joinedBookings = from booking in bookings
                             join vehicle in vehicles on booking.RegistrationNumber equals vehicle.RegistrationNumber
                             select new { Booking = booking, Vehicle = vehicle };

        foreach (var item in joinedBookings)
        {
            item.Booking.Vehicle = item.Vehicle;
        }
    }

    private void SetOdometerPosition()
    {
        var bookings = _dataService.GetDataObjectsOfType<IBooking>();
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
        var bookings = _dataService.GetDataObjectsOfType<IBooking>();

        foreach (var booking in bookings)
        {
            if (booking.Vehicle != null)
            {
                double costPerKilometer = booking.Vehicle.CostPerKilometer;
                double dailyRate = booking.Vehicle.DailyRate;
                double odometerStart = booking.OdometerStart;
                double odometerEnd = booking.OdometerEnd;

                int rentalDays = (booking.EndDate - booking.StartDate).Days;
                double totalKilometerCost = costPerKilometer * (odometerEnd - odometerStart);
                double totalDailyCost = dailyRate * rentalDays;
                double totalCost = totalKilometerCost + totalDailyCost;

                booking.RentalDays = rentalDays;
                booking.TotalKilometers = odometerEnd - odometerStart;
                totalCost = Math.Round(totalCost, 2);
                booking.TotalCost = totalCost;
                booking.TotalKilometerCost = totalKilometerCost;
                booking.TotalDailyCost = totalDailyCost;
            }
        }

    }
}


