using Helpers;
using Model;
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
        public KeyValuePair<string, double> GetLastCurrency(string currency)
        {
            var exrateService = new ExRatesService();
            ExRatesModel rate;
            try
            {
                rate = exrateService.GetRate(СurrencyEnumHelper.GetCurrencyByName(currency), DateTime.Now.AddDays(1));
            }
            catch (Exception)
            {
                try
                {
                    rate = exrateService.GetRate(СurrencyEnumHelper.GetCurrencyByName(currency), DateTime.Now);
                }
                catch (Exception)
                {

                    return new KeyValuePair<string, double>("Нет подключения к сети", 0);
                }           
            }

            var result = new KeyValuePair<string, double>(DateHelper.ConvertToUiStringDate(rate.Date), rate.Rate);
            return result;

        }

        public KeyValuePair<string, double> GetCurrency(string currency, string date)
        {
            var exrateService = new ExRatesService();
            ExRatesModel rate;
            try
            {
                var dateTime = DateHelper.ConvertToDateFromUiString(date);
                rate = exrateService.GetRate(СurrencyEnumHelper.GetCurrencyByName(currency), dateTime.Value.AddDays(-1));
            }
            catch (Exception)
            {
                return new KeyValuePair<string, double>("Нет подключения к сети", 0);
            }

            var result = new KeyValuePair<string, double>(DateHelper.ConvertToUiStringDate(rate.Date), rate.Rate);
            return result;
        }

        public IList<KeyValuePair<long, double>> GetCurencesList(string currency)
        {
            var exrateService = new ExRatesService();
            var rates = exrateService.GetRatePeriod(СurrencyEnumHelper.GetCurrencyByName(currency), DateTime.Now.AddMonths(-1), DateTime.Now.AddDays(1));
            if (rates == null)
            {
                return new List<KeyValuePair<long, double>>();
            }

            var result = rates.Select(exRatesModel =>
                                      new KeyValuePair<long, double>(DateHelper.ConvertToTimestamp(exRatesModel.Date), 
                                                                  exRatesModel.Rate)).ToList();
            return result;
        }
    }
}
