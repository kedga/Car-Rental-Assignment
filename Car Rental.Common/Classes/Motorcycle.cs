using Car_Rental.Common.Utilities;

namespace Car_Rental.Common.Classes;

public class Motorcycle : Vehicle
{
    public Motorcycle(string registrationNumber, string make) : base(registrationNumber, make)
    {
    }
    private string _engineSizeCC = string.Empty;

    public string EngineSizeCC
    {
        get { return _engineSizeCC; }
        set { _engineSizeCC = value.FormatAsInt(); }
    }
}