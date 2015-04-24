using Helpers;
using NetworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace UICore
{
    public sealed class Binding
    {
        public KeyValuePair<string, int> GetLastCurrency()
        {
            var exrateService = new ExRates();
            var rate = exrateService.GetRate(DateTime.Now.AddDays(1));
            var result = new KeyValuePair<string, int>(DateHelper.ConvertToUiStringDate(rate.Date), rate.Rate);
            return result;

        }

        public IList<KeyValuePair<long, int>> GetCurencesList()
        {
            var exrateService = new ExRates();
            var rates = exrateService.GetRatePeriod(DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(1));
            var result = rates.Select(exRatesModel =>
                                      new KeyValuePair<long, int>(DateHelper.ConvertToTimestamp(exRatesModel.Date), 
                                                                  exRatesModel.Rate)).ToList();
            return result;
        }
    }
}
