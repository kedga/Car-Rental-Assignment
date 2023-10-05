using Car_Rental.Common.Utilities;

namespace Car_Rental.Common.Classes;

public class Motorcycle : Vehicle
{
    public Motorcycle(string registrationNumber, string make) : base(registrationNumber, make)
    {
    }
    public int EngineSizeCC { get; set; }

    public string EngineSizeCCInput
    {
        get
        {
            if (EngineSizeCC > 0) return EngineSizeCC.ToString();
            else return string.Empty;
        }
        set { EngineSizeCC = value.FormatAndParseAsInt(); }
    }
}