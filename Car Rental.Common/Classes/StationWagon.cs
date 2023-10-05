using Car_Rental.Common.Utilities;

namespace Car_Rental.Common.Classes;

public class StationWagon : Vehicle
{
    public StationWagon(string registrationNumber, string make) : base(registrationNumber, make)
    {
    }
    public int TowingCapacity { get; set; }

    public string TowingCapacityInput
    {
        get
        {
            if (TowingCapacity > 0) return TowingCapacity.ToString();
            else return string.Empty;
        }
        set { TowingCapacity = value.FormatAndParseAsInt(); }
    }
}