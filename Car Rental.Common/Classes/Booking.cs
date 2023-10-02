using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;


namespace Car_Rental.Common.Classes;

public class Booking : IBooking
{
    public Booking(IPerson customer, IVehicle vehicle, DateTime startDate)
    {
        Customer = customer;
        Vehicle = vehicle;
        StartDate = startDate;
    }
    public int Id { get; set; } = 0;
    public string RegistrationNumber { get; set; } = string.Empty;
    public string CustomerSsn { get; set; } = string.Empty;
    public double OdometerStart { get; set; } = 0.0;
    public double OdometerEnd { get; set; } = 0.0;
    public DateTime StartDate { get; set; } = DateTime.MinValue;
    public DateTime EndDate { get; set; } = DateTime.MinValue;
    public BookingStatus BookingStatus { get; set; } = BookingStatus.Open;
    public double TotalKilometers { get; set; } = 0.0;
    public decimal TotalCost { get; set; } = 0.0m;
    public decimal TotalKilometerCost { get; set; } = 0.0m;
    public decimal TotalDailyCost { get; set; } = 0.0m;
    public int RentalDays { get; set; } = 0;
    public IVehicle Vehicle { get; set; } = new Vehicle("","");
    public IPerson Customer { get; set; } = new Person("", "", "");
    
}
