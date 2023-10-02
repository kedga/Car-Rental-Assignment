using Car_Rental.Common.Enums;

namespace Car_Rental.Common.Interfaces;

public interface IVehicle : IDataObject
{
    string RegistrationNumber { get; set; }
    string Make { get; set; }
    double OdometerPosition { get; set; }
    decimal CostPerKilometer { get; set; }
    VehicleType VehicleType { get; set; }
    decimal DailyRate { get; set; }
    BookingStatus BookingStatus { get; set; }
    int LastBookingId { get; set; }
}
