using Car_Rental.Common.Utilities;

namespace Car_Rental.Business.Classes;

public static class DataValidation
{
    public static string ValidateAndReturnCssClass(object? inputData, string type, bool aggressive, string validClass = "is-valid", string invalidClass = "is-invalid", string nullClass = "is-null")
    {
        string defaultClass = aggressive ? invalidClass : nullClass;

        if (inputData == null) return defaultClass;

        string inputString = inputData.ToString();

        if (string.IsNullOrEmpty(inputString))
        {
            return defaultClass;
        }

        switch (type.ToLower())
        {
            case "mixed":
                if (inputData.ToString().HasDigitsAndLetters()) return validClass;
                break;

            case "int":
                if (int.TryParse(inputData.ToString(), out int intValue) && intValue.IsPositive()) return validClass;
                break;

            case "double":
                if (double.TryParse(inputData.ToString(), out double doubleValue) && doubleValue.IsPositive()) return validClass;
                break;

            case "only letters":
                if (inputData.ToString().ContainsOnlyLetters()) return validClass;
                break;

            case "string":
                if (!string.IsNullOrWhiteSpace(inputData.ToString())) return validClass;
                break;

            case "ssn":
                if (inputData is bool ssnBool)
                {
                    if (ssnBool == true) return validClass;
                    else if (ssnBool == false) return invalidClass;
                    break;
                }
                else
                {
                    return invalidClass;
                }

            default:
                break;
        }

        return defaultClass;
    }
}