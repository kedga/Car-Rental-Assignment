using Car_Rental.Common.Utilities;

namespace Car_Rental.Common.Classes;

public class Sedan : Vehicle
{
    public Sedan(string registrationNumber, string make) : base(registrationNumber, make)
    {
    }
    private string _topSpeed = string.Empty;

    public string TopSpeed
    {
        get { return _topSpeed; }
        set { _topSpeed = value.FormatAsInt(); }
    }
}