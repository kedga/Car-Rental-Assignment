using Car_Rental.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Car_Rental.Common.Interfaces;

namespace Car_Rental.Common.Classes;

public class Vehicle : IVehicle
    {
	public Vehicle(string registrationNumber, string make)
	{
		RegistrationNumber = registrationNumber;
		Make = make;
	}

	public string RegistrationNumber { get; set; }
	public string Make { get; set; }
	public double OdometerPosition { get; set; }
    public double? OdometerPositionNullable { get; set; }
    public double CostPerKilometer { get; set; }
    public double? CostPerKilometerNullable { get; set; }
    public VehicleType VehicleType { get; set; }
    public double DailyRate { get; set; }
    public double? DailyRateNullable { get; set; }
    public BookingStatuses BookingStatus { get; set; }
}
