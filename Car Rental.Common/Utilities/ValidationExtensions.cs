using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Utilities;

public static class ValidationExtensions
{
    public static bool IsPositive(this int value)
    {
        return value >= 0;
    }
    public static bool IsPositive(this int? value)
    {
        return value >= 0;
    }
    public static bool IsPositive(this double value)
    {
        return value >= 0;
    }
    public static bool IsPositive(this double? value)
    {
        return value >= 0;
    }
    public static bool isPositive(this string value)
    {
        if (double.TryParse(value, out double numericValue))
        {
            return numericValue > 0;
        }
        return false;
    }
    public static bool HasDigitsAndLetters(this string inputData)
    {
        bool containsNumber = false;
        bool containsLetter = false;

        foreach (char character in inputData)
        {
            if (char.IsDigit(character)) containsNumber = true;
            else if (char.IsLetter(character)) containsLetter = true;

            if (containsNumber && containsLetter) return true;
        }

        return false;
    }
    public static bool ContainsOnlyLetters(this string inputData)
    {
        bool containsLetter = false;

        foreach (char character in inputData)
        {
            if (char.IsLetter(character)) containsLetter = true;
            else if (!char.IsWhiteSpace(character)) return false;
        }

        return containsLetter;
    }
}
