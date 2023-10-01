namespace Car_Rental.Common.Classes;

public class StationWagon : Vehicle
{
    public StationWagon(string registrationNumber, string make) : base(registrationNumber, make)
    {
    }
    public string NumberOfSeats { get; set; } = string.Empty;

}