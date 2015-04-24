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
        public IList<string> tt { get; set; }
        public string GetTestString()
        {
            var t = new ExRates();
            //var rate = t.GetRatePeriod(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(1));
            var rate = t.GetRate(DateTime.Now);
            return rate.Rate.ToString();

        }

        public IList<int> GetCurencesList()
        {
            var exrateService = new ExRates();
            //DateTime dateNow = DateTime.Now;
            //var firstMonthDay = new DateTime(dateNow.Year, dateNow.Month, 1);

            var rates = exrateService.GetRatePeriod(DateTime.Now.AddMonths(-1), DateTime.Now);
            return rates.Select(r => r.Rate).ToList();
        }
    }
}
