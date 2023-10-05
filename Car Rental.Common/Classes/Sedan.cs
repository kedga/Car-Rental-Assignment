using Car_Rental.Common.Utilities;

namespace Car_Rental.Common.Classes;

public class Sedan : Vehicle
{
    public Sedan(string registrationNumber, string make) : base(registrationNumber, make)
    {
    }
    public int TopSpeed { get; set; }

    public string TopSpeedInput
    {
        get
        {
            if (TopSpeed > 0) return TopSpeed.ToString(); 
            else return string.Empty;
        }
        set { TopSpeed = value.FormatAndParseAsInt(); }
    }
}