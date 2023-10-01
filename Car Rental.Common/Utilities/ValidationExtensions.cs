using System.Text.RegularExpressions;

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
    public static bool IsPositive(this string value)
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
    public static bool ValidSsn(this string ssn)
    {
        if (Regex.IsMatch(ssn, @"^(19|20)(0[1-9]|[1-9][0-9])(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])(-?[0-9]{4})$"))
            return true;
        else
            return false;
    }

    /// <summary>
    /// Checks if a string collides with any string in a collection.
    /// </summary>
    /// <param name="inputString">The string to check for collisions.</param>
    /// <param name="collection">The collection of strings to check against.</param>
    /// <param name="pattern">Ignore any characters not uncluded in this pattern.</param>
    /// <param name="ignoreCase">True to perform a case-insensitive comparison, false for case-sensitive.</param>
    /// <returns>True if there is a collision, otherwise false.</returns>
    public static bool StringCollisionCheck(this string inputString, IEnumerable<string> collection, string pattern = ".*", bool ignoreCase = false)
    {
        string filteredInputString = string.Join("", Regex.Matches(inputString, pattern).Cast<Match>().Select(match => match.Value));

        StringComparer comparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

        return collection
            .Any(value => comparer.Equals(
                string.Join("", Regex.Matches(value, pattern).Cast<Match>().Select(match => match.Value)),
                filteredInputString));
    }
}
