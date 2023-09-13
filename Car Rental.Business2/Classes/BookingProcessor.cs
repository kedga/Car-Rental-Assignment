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
    private IEnumerable<IVehicle>? _vehicles;
    private IEnumerable<IBooking>? _bookings;
    private IEnumerable<IPerson>? _people;
    public BookingProcessor(IData dataService)
    {
        _dataService = dataService;
    }

    public async Task GetAllDataAsync()
    {
        _bookings = await _dataService.GetBookingsAsync();
        _vehicles = await _dataService.GetVehiclesAsync();
        _people = await _dataService.GetPeopleAsync();
        MatchBookingsWithVehicles();
        SetOdometerPosition();
        CalculateTotalCost();
    }
    public IEnumerable<IBooking> GetBookings()
    {
        return _bookings;
    }

    public IEnumerable<IVehicle> GetVehicles()
    {
        return _vehicles;
    }

    public IEnumerable<IPerson> GetCustomers()
    {
        return _people;
    }

    private void MatchBookingsWithVehicles()
    {
        var joinedBookings = from booking in _bookings
                             join vehicle in _vehicles on booking.RegistrationNumber equals vehicle.RegistrationNumber
                             select new { Booking = booking, Vehicle = vehicle };

        foreach (var item in joinedBookings)
        {
            item.Booking.Vehicle = item.Vehicle;
        }
    }

    private void SetOdometerPosition()
    {
        var groupedBookings = _bookings.GroupBy(booking => booking.Vehicle);

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
        foreach (var booking in _bookings)
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


