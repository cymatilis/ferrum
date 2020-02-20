using System;
using System.ComponentModel.DataAnnotations;
using Ferrum.Core.Extensions;

namespace Ferrum.Core.Validation.CardDate
{
    public enum CardDateType
    {
        StartDate,
        ExpiryDate
    }
    
    /// <summary>
    /// Validate a 4 digit card date as start date or expiry date.
    /// </summary>
    public class CardDateAttribute : ValidationAttribute
    {
        public CardDateType CardDateType { get; set; }

        public CardDateAttribute(CardDateType cardDateType)
        {
            CardDateType = cardDateType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value.ToString().Length == 0)
                return new ValidationResult($"The {validationContext.DisplayName} field is required.");

            var validationWrapper = MonthYearValidation.Parse(value.ToString());
            if (!validationWrapper.IsValid)
                return new ValidationResult(validationWrapper.Error);

            var my = validationWrapper.MonthYear;

            var firstDayOfMonth = new DateTime(2000 + my.Year, my.Month, 1);

            switch (CardDateType)
            {
                case CardDateType.StartDate:
                    var fromDate = new DateTime(2000 + my.Year, my.Month, 1);
                    if (fromDate > DateTime.UtcNow.Date)
                        return new ValidationResult("Start Date cannot be in the future.");

                    if (DateTime.UtcNow.Date.Year - fromDate.Year > 20)
                        return new ValidationResult("Start Date cannot be over 20 years in the past.");
                    break;

                case CardDateType.ExpiryDate:
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    if (lastDayOfMonth < DateTime.UtcNow.Date)
                        return new ValidationResult("Incorrect expiry date or card has expired.");

                    if (lastDayOfMonth.Year - DateTime.UtcNow.Date.Year > 20)
                        return new ValidationResult("Expiry Date cannot be over 20 years in the future.");
                    break;                   
            }

            return ValidationResult.Success;
        }
    }

    /// <summary>
    /// Represents any date with only a 2 digit year and 2 digit month
    /// </summary>
    public struct MonthYear
    {
        public byte Month { get; set; }
        public byte Year { get; set; }
    }

    internal class MonthYearValidationWrapper
    {
        internal MonthYear MonthYear { get; set; }
        internal string Error { get; set; }
        internal bool IsValid { get; set; }
    }

    internal static class MonthYearValidation
    {
        internal static MonthYearValidationWrapper Parse(string monthYear)
        {
            if (monthYear.Length < 4 || monthYear.Length > 5)
                return WrongFormat(monthYear);

            byte month = 0;
            byte year = 0;

            //MMYY
            if (monthYear.Length == 4)
            {
                month = byte.Parse(monthYear.Substring(0, 2));
                year = byte.Parse(monthYear.Substring(2, 2));
            }

            //MM/YY or MM-YY
            if (monthYear.Length == 5)
            {
                var seperator = monthYear.Substring(2, 1);
                if (seperator.NotIn("-", "/"))
                    return WrongFormat(monthYear);

                month = byte.Parse(monthYear.Substring(0, 2));
                year = byte.Parse(monthYear.Substring(3, 2));
            }

            if (month > 12)
                return WrongFormat(monthYear);
            
            return Valid(month, year);
        }
        internal static MonthYearValidationWrapper WrongFormat(string monthYear)
        {
            return new MonthYearValidationWrapper
            {
                IsValid = false,
                Error = $"{monthYear} is not in an accepted date format. Accepted date formats are MMYY, MM-YY or MM/YY."
            };
        }
        internal static MonthYearValidationWrapper Valid(byte month, byte year)
        {
            return new MonthYearValidationWrapper
            {
                MonthYear = new MonthYear { Month = month, Year = year },
                IsValid = true
            };
        }
    }
}
