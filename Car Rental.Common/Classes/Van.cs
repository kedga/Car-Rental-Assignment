using Car_Rental.Common.Utilities;

namespace Car_Rental.Common.Classes;

public class Van : Vehicle
{
    public Van(string registrationNumber, string make) : base(registrationNumber, make)
    {
    }
    private string _cargoCapacityCubicMeters = string.Empty;
    public string CargoCapacityCubicMeters
    {
        get { return _cargoCapacityCubicMeters; }
        set { _cargoCapacityCubicMeters = value.FormatAsDouble(2); }
    }
}