using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;


namespace Car_Rental.Common.Classes;

public class Booking : IBooking
{
    public Booking(string customerFirstName, string customerLastName, string registrationNumber, string customerSsn)
    {
        CustomerFirstName = customerFirstName;
        CustomerLastName = customerLastName;
        RegistrationNumber = registrationNumber;
        CustomerSsn = customerSsn;
    }
    public string RegistrationNumber { get; set; }
    public string CustomerSsn { get; set; }
    public string CustomerFirstName { get; set; }
    public string CustomerLastName { get; set; }
    public double OdometerStart { get; set; }
	public double OdometerEnd { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public BookingStatuses BookingStatus { get; set; }
    public double TotalKilometers { get; set; }
    public double TotalCost { get; set; }
    public double TotalKilometerCost { get; set; }
    public double TotalDailyCost { get; set; }
	public int RentalDays { get; set; }
    public IVehicle? Vehicle { get; set; }
    public IPerson Customer { get; set; }
}
