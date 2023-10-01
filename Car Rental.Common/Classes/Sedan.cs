namespace Car_Rental.Common.Classes;

public class Sedan : Vehicle
{
    public Sedan(string registrationNumber, string make) : base(registrationNumber, make)
    {
    }
    public string TopSpeed { get; set; } = string.Empty;
}