using Car_Rental.Common.Enums;

namespace Car_Rental.Common.Interfaces;

public interface IBooking : IDataObject
{
    int Id { get; set; }
    string RegistrationNumber { get; set; }
    string CustomerSsn { get; }
    double OdometerStart { get; }
    double OdometerEnd { get; set; }
    DateTime StartDate { get; set; }
    DateTime EndDate { get; set; }
    BookingStatus BookingStatus { get; set; }
    double TotalKilometers { get; set; }
    decimal TotalCost { get; set; }
    decimal TotalKilometerCost { get; set; }
    decimal TotalDailyCost { get; set; }
    int RentalDays { get; set; }
    IVehicle Vehicle { get; set; }
    IPerson Customer { get; set; }
}

