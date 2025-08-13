using System.ComponentModel.DataAnnotations;
using PrescribingSystem.Models;

namespace PrescribingSystem.Models
{

public class SouthAfricanIdAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string idNumber = value?.ToString();

        if (string.IsNullOrEmpty(idNumber) || !IsValidSouthAfricanId(idNumber))
            return new ValidationResult("Invalid South African ID number.");

        return ValidationResult.Success;
    }

    private bool IsValidSouthAfricanId(string idNumber)
    {
        if (idNumber.Length != 13 || !long.TryParse(idNumber, out _))
            return false;

        string year = idNumber.Substring(0, 2);
        string month = idNumber.Substring(2, 2);
        string day = idNumber.Substring(4, 2);

        if (!DateTime.TryParseExact($"{year}{month}{day}", "yyMMdd", null, System.Globalization.DateTimeStyles.None, out _))
            return false;

        return LuhnCheck(idNumber);
    }

    private bool LuhnCheck(string number)
    {
        int sum = 0;
        bool alternate = false;

        for (int i = number.Length - 1; i >= 0; i--)
        {
            int n = int.Parse(number[i].ToString());
            if (alternate)
            {
                n *= 2;
                if (n > 9)
                    n -= 9;
            }
            sum += n;
            alternate = !alternate;
        }
        return (sum % 10 == 0);
    }
}
}

