namespace Car_Rental.Common.Enums;

public enum VehicleType
{
    None = 0,
    Sedan = 1,
    Van = 2,
    Motorcycle = 3,
    StationWagon = 4
}
public static class VehicleTypeExtensions
{
    public static string DisplayName(this VehicleType vehicleType)
    {
        switch (vehicleType)
        {
            case VehicleType.None:
                return "Select vehicle type";
            case VehicleType.Sedan:
                return "Sedan";
            case VehicleType.Van:
                return "Van";
            case VehicleType.Motorcycle:
                return "Motorcycle";
            case VehicleType.StationWagon:
                return "Station wagon";
            default:
                return string.Empty;
        }
    }
}