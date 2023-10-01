using System.Globalization;

namespace Car_Rental.Common.Utilities;

public static class TimeExtensions
{
    public static int GetAgeFromSsn(this string ssn, string format = "yyyyMMdd")
    {
        if (DateTime.TryParseExact(ssn.Substring(0, 8), format, null, DateTimeStyles.None, out DateTime birthdate))
        {
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - birthdate.Year;

            if (birthdate.Date > currentDate.AddYears(-age)) age--;

            return age;
        }
        else return -1;
    }
    public static int DaysRoundedUp(this DateTime startDate, DateTime endDate)
    {
        return (int)Math.Ceiling((endDate - startDate).TotalHours / 24);
    }
    public static DateTime RoundToMinutes(this DateTime dateTime)
    {
        return dateTime.Date.AddMinutes(dateTime.Hour * 60 + dateTime.Minute);
    }

}
