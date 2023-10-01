using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;

namespace Car_Rental.Common.Classes;

public class Vehicle : IVehicle, IDataObject
    {
	public Vehicle(string registrationNumber, string make)
	{
		_registrationNumber = registrationNumber;
		Make = make;
	}
    private string _registrationNumber;
    public string RegistrationNumber
    {
        get { return _registrationNumber; }
        set { _registrationNumber = value.ToUpper(); }
    }
    public string Make { get; set; }
	public double OdometerPosition { get; set; }
    public decimal CostPerKilometer { get; set; }
    public VehicleType VehicleType { get; set; }
    public decimal DailyRate { get; set; }
    public BookingStatuses BookingStatus { get; set; }
    public int LastBookingId { get; set; }
    public virtual Dictionary<string, string> UniquePropertiesDict()
    {
        return new Dictionary<string, string>();
    }
}
