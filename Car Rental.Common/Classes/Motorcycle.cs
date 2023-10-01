namespace Car_Rental.Common.Classes;

public class Motorcycle : Vehicle
{
    public Motorcycle(string registrationNumber, string make) : base(registrationNumber, make)
    {
    }
    public string EngineSizeCC { get; set; } = string.Empty;
}