namespace Car_Rental.Common.Utilities;
public static class InputStringConversionExtensions
{
    public static int FormatAndParseAsInt(this string input)
    {
        if (int.TryParse(input.FormatAsInt(), out int result))
            return result;
        else return 0;
    }
    public static double FormatAndParseAsDouble(this string input, int maxDecimals)
    {
        if (double.TryParse(input.FormatAsDouble(maxDecimals), out double result)) 
            return result;
        else return 0;
    }
}