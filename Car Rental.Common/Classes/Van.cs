using Car_Rental.Common.Utilities;

namespace Car_Rental.Common.Classes;

public class Van : Vehicle
{
    public Van(string registrationNumber, string make) : base(registrationNumber, make)
    {
    }
    public double CargoCapacity { get; set; }

    private string _cargoCapacityString = string.Empty;
    public string CargoCapacityInput
    {
        get
        {
            return _cargoCapacityString;
        }
        set
        {
            CargoCapacity = value.FormatAndParseAsDouble(2);
            _cargoCapacityString = value.FormatAsDouble(2);
        }
    }
}