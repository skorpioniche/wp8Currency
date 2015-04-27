using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Helpers
{
    public static class СurrencyEnumHelper
    {
        public static СurrencyEnum GetCurrencyByName(string currencyName)
        {
            return (СurrencyEnum)Enum.Parse(typeof(СurrencyEnum), currencyName);
        }
    }
}
