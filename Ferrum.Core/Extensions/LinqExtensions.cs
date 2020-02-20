using System.Linq;

namespace Ferrum.Core.Extensions
{
    public static class LinqExtensions
    {
        public static bool In(this byte value, params byte[] values)
        {
            return values.Contains(value);
        }

        public static bool In(this string value, params string[] values)
        {
            return values.Contains(value);
        }

        public static bool In(this char value, params char[] values)
        {
            return values.Contains(value);
        }

        public static bool NotIn(this string value, params string[] values)
        {
            return !values.Contains(value);
        }

        public static bool IsNumeric(this string value)
        {
            var result = value.All(c => c.In('0', '1', '2', '3', '4', '5', '6', '7', '8', '9'));
            return result;
        }
    }
}
