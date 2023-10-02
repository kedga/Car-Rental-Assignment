using System.Globalization;
using System.Text.RegularExpressions;

namespace Car_Rental.Common.Utilities;

public static class FormattingExtensions
{
    public static string FormatAsCurrency(this decimal amount)
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
    public static string FormatAsInt(this string inputString)
    {
        return Regex.Replace(inputString, "[^0-9]", "");
    }
    public static string FormatAsDouble(this string inputString, int maxDecimals = -1)
    {
        inputString = inputString.Replace(',', '.');
        inputString = Regex.Replace(inputString, "[^0-9.]", "");

        int decimalPointIndex = inputString.IndexOf('.');

        if (decimalPointIndex >= 0)
        {
            string integralPart = inputString.Substring(0, decimalPointIndex);
            string fractionalPart = inputString.Substring(decimalPointIndex + 1);

            fractionalPart = fractionalPart.Replace(".", "");

            if (maxDecimals >= 0 && fractionalPart.Length > maxDecimals)
            {
                fractionalPart = fractionalPart.Substring(0, maxDecimals);
            }

            inputString = integralPart + "." + fractionalPart;
        }

        return inputString;
    }


}