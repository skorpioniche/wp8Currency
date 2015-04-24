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
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public const string DateFormatForServices = "MM/dd/yyyy";

        public const string DateFormatUi = "dd.MM.yyyy";

        public static DateTime? ConvertToDate(string dateString)
        {
            var dateValue = new DateTime();
            if (DateTime.TryParseExact(dateString, DateFormatForServices,
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
            return date.ToString(DateFormatForServices);
        }

        public static string ConvertToUiStringDate(DateTime date)
        {
            return date.ToString(DateFormatUi);
        }

        
        public static long ConvertToTimestamp(DateTime date)
        {
            TimeSpan elapsedTime = date - Epoch;
            return (long) elapsedTime.TotalSeconds*1000;
        }
    }
}
