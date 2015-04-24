using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class DateHelper
    {
        public const string DateFormat = "MM/dd/yyyy";

        public static DateTime? ConvertToDate(string dateString)
        {
            var dateValue = new DateTime();
            if (DateTime.TryParseExact(dateString, DateFormat,
                new CultureInfo("en-US"),
                DateTimeStyles.None,
                out dateValue))
            {
                return dateValue;
            }
            else
            {
                return null;
            }
        }

        public static string ConvertToStringDate(DateTime date)
        {
            return date.ToString(DateFormat);
        }
    }
}
