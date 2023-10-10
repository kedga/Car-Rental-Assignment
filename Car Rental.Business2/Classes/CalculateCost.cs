using Car_Rental.Common.Interfaces;
using Car_Rental.Common.Utilities;

namespace Car_Rental.Business.Classes;

public static class CalculateCost
{
	public static void CalculateTotalCost(this IBooking booking)
	{
		if (booking.Vehicle == null) return;

		booking.RentalDays = booking.StartDate.DaysRoundedUp(booking.EndDate);
		decimal totalKilometerCost = booking.Vehicle.CostPerKilometer * ((decimal)booking.OdometerEnd - (decimal)booking.OdometerStart);
		decimal totalDailyCost = booking.Vehicle.DailyRate * booking.RentalDays;
		decimal totalCost = totalKilometerCost + totalDailyCost;

		booking.TotalKilometers = booking.OdometerEnd - booking.OdometerStart;
		booking.TotalCost = totalCost;
		booking.TotalKilometerCost = totalKilometerCost;
		booking.TotalDailyCost = totalDailyCost;
	}
}
