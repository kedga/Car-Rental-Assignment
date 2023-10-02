using Car_Rental.Common.Utilities;

namespace Car_Rental.Common.Classes;

public class StationWagon : Vehicle
{
    public StationWagon(string registrationNumber, string make) : base(registrationNumber, make)
    {
    }
    private string _numberOfSeats = string.Empty;

    public string NumberOfSeats
    {
        get { return _numberOfSeats; }
        set { _numberOfSeats = value.FormatAsInt(); }
    }
}