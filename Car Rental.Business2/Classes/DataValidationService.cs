using Car_Rental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.RegularExpressions;
using Car_Rental.Common.Utilities;
using System.Threading.Tasks;

namespace Car_Rental.Business.Classes;

public class DataValidationService
{
    public Dictionary<int, bool> FinalCheckDict { get; set; } = Enumerable.Range(1, 10).ToDictionary(x => x, x => false);
    public string ValidateAndReturnCssClass(object? inputData, string type, int id, string validClass = "is-valid", string invalidClass = "is-invalid", string nullClass = "is-null")
    {
        string defaultClass = FinalCheckDict[id] ? invalidClass : nullClass;

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

            case "number":
                if (int.TryParse(inputData.ToString(), out int intValue) && intValue.IsPositive()) return validClass;
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