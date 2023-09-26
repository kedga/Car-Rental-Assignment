using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Utilities;

public static class DateTimeUtils
{
    public static int GetAgeFromSsn(string ssn)
    {
        string format = "yyyyMMdd";

        if (DateTime.TryParseExact(ssn.Substring(0, 8), format, null, DateTimeStyles.None, out DateTime birthdate))
        {
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - birthdate.Year;

            if (birthdate.Date > currentDate.AddYears(-age)) age--;

            return age;
        }
        else return -1;
    }
    public static int Duration(this DateTime endDate, DateTime startDate)
    {
        return (endDate - startDate).Days;
    }

}
