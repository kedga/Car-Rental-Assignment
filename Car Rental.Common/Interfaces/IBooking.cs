using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;

namespace Car_Rental.Common.Interfaces;

public interface IBooking : IDataObject
{
    string RegistrationNumber { get; set; }
    string CustomerName { get; }
    double OdometerStart { get; }
    double OdometerEnd { get; }
    DateTime StartDate { get; }
    DateTime EndDate { get; }
    BookingStatuses BookingStatus { get; }
    double TotalKilometers { get; set; }
    double TotalCost { get; set; }
    double TotalKilometerCost { get; set; }
    double TotalDailyCost { get; set; }
    int RentalDays { get; set; }
    IVehicle Vehicle { get; set; }
}

