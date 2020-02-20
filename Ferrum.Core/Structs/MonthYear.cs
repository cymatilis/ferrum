using System;
using Ferrum.Core.Extensions;

namespace Ferrum.Core.Structs
{
    public struct MonthYear
    {
        public byte Month { get; set; }
        public byte Year { get; set; }

        public MonthYear(string monthYear)
        {
            this = MonthYearValidation.Parse(monthYear);
        }
    }

    internal static class MonthYearValidation
    {
        internal static void WrongFormat()
        {
            throw new ArgumentException("Month Year is not in an accpeted format.", "monthYear");
        }

        internal static MonthYear Parse(string monthYear)
        {
            if (monthYear.Length < 4 || monthYear.Length > 7)
                WrongFormat();

            byte month = 0;
            byte year = 0;

            //MMYY
            if(monthYear.Length == 4)
            {
                month = byte.Parse(monthYear.Substring(0, 2));
                year = byte.Parse(monthYear.Substring(2, 2));
            }

            //MM/YY or MM-YY
            if(monthYear.Length == 5)
            {
                var seperator = monthYear.Substring(2, 1);
                if (seperator.NotIn("-", "/"))
                    WrongFormat();

                month = byte.Parse(monthYear.Substring(0, 2));
                year = byte.Parse(monthYear.Substring(3, 2));
            }

            //MMYYYY
            if(monthYear.Length == 6)
            {
                month = byte.Parse(monthYear.Substring(0, 2));
                year = byte.Parse(monthYear.Substring(3, 2));
            }

            //MM/YYYY or MM-YYYY
            if (monthYear.Length == 7)
            {
                var seperator = monthYear.Substring(2, 1);
                if (seperator.NotIn("-", "/"))
                    WrongFormat();

                month = byte.Parse(monthYear.Substring(0, 2));
                year = byte.Parse(monthYear.Substring(4, 2));
            }

            if (month > 12)
                WrongFormat();

            return new MonthYear { Month = month, Year = year };
        }
    }
}
