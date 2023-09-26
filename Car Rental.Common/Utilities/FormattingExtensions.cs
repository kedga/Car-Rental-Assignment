using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Car_Rental.Common.Utilities;

public static class FormattingExtensions
{
    public static string FormatAsCurrency(this double amount)
    {
        string formattedAmount = amount.ToString("C", CultureInfo.CreateSpecificCulture("fr-FR")); //"sv-SE" is not working properly for some reason
        return formattedAmount.Replace("€", "kr");
    }
    public static (string, bool?) FormatSsn(this string ssn)
    {
        bool? validSsn = null;
        if (Regex.IsMatch(ssn, @"^(0[1-9]|[1-9][0-9])(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])(-?[0-9]{4})$"))
        {
            validSsn = true;
            int currentYear = DateTime.Now.Year % 100;
            int.TryParse(ssn.Substring(0, 2), out int ssnYear);
            if (ssnYear > currentYear)
                ssn = "19" + ssn;
            else
                ssn = "20" + ssn;
        }
        else if (Regex.IsMatch(ssn, @"^(19|20)(0[1-9]|[1-9][0-9])(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])(-?[0-9]{4})$"))
            validSsn = true;
        else
            validSsn = null;
        if (validSsn == true && ssn.Length == 12)
            ssn = ssn.Insert(8, "-");
        return (ssn, validSsn);
    }
}