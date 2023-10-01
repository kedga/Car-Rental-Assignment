namespace Car_Rental.Common.Classes;

public class Van : Vehicle
{
    public Van(string registrationNumber, string make) : base(registrationNumber, make)
    {
    }
    public string CargoCapacityCubicMeters { get; set; } = string.Empty;

}