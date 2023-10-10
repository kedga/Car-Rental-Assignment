using Car_Rental.Common.Enums;
using Car_Rental.Common.Utilities;

namespace Car_Rental.Business.Classes;

public static class DataValidation
{
    public static string ValidateAndReturnCssClass(this object? inputData,
                                                   ValidationType type,
                                                   bool aggressive,
                                                   string validClass = "is-valid",
                                                   string invalidClass = "is-invalid",
                                                   string nullClass = "is-null")
    {
        string defaultClass = aggressive ? invalidClass : nullClass;

        if (inputData == null) return defaultClass;

        string inputString = inputData.ToString()!;

        if (string.IsNullOrEmpty(inputString))
        {
            return defaultClass;
        }

        switch (type)
        {
            case ValidationType.Mixed:
                if (inputData.ToString()!.HasDigitsAndLetters()) return validClass;
                break;

            case ValidationType.Int:
                if (int.TryParse(inputData.ToString(), out int intValue) && intValue.IsPositive()) return validClass;
                break;

            case ValidationType.Double:
                if (double.TryParse(inputData.ToString(), out double doubleValue) && doubleValue.IsPositive()) return validClass;
                break;
                
            case ValidationType.OnlyLetters:
                if (inputData.ToString()!.ContainsOnlyLetters()) return validClass;
                break;

            case ValidationType.String:
                if (!string.IsNullOrWhiteSpace(inputData.ToString())) return validClass;
                break;

            case ValidationType.SSN:
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

            case ValidationType.VehicleType:
                if (inputData is VehicleType vehicleType && vehicleType is VehicleType.None)
                {
                    return defaultClass;
                }
                else
                {
                    return validClass;
                }

            default:
                break;
        }

        return defaultClass;
    }
}