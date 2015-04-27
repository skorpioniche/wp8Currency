using System.Globalization;
using Windows.Globalization.NumberFormatting;
using Helpers;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Data.Xml.Dom;

namespace NetworkCore
{
    public class ExRatesService
    {
        public ExRatesModel GetRate(СurrencyEnum currency, DateTime date)
        {
            string xml = string.Empty;
            var url = new Uri("http://www.nbrb.by/Services/XmlExRates.aspx?ondate=" + DateHelper.ConvertToStringDate(date));
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
            var response = httpClient.GetAsync(url).Result;

            using (var responseStream = response.Content.ReadAsStreamAsync().Result)
            using (var streamReader = new StreamReader(responseStream))
            {
                xml = streamReader.ReadToEnd();
            }
            XDocument xDoc = XDocument.Parse(xml);

            XElement xmlDataElement = (from xml2 in xDoc.Descendants("Currency")
                                       where xml2.Element("CharCode").Value == currency.ToString()
                                       select xml2).FirstOrDefault();

            var result = new ExRatesModel
            {
                Rate = double.Parse(xmlDataElement.Element("Rate").Value, CultureInfo.InvariantCulture),
                Date = date
            };

            return result;
        }

        public List<ExRatesModel> GetRatePeriod(СurrencyEnum currency, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                string xml = string.Empty;
                var url = new Uri("http://www.nbrb.by/Services/XmlExRatesDyn.aspx?curId="+
                                  + (int)currency + "&fromDate="
                                  + DateHelper.ConvertToStringDate(dateFrom) + "&toDate=" +
                                  DateHelper.ConvertToStringDate(dateTo));
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept",
                    "text/html,application/xhtml+xml,application/xml");
                var response = httpClient.GetAsync(url).Result;

                using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                using (var streamReader = new StreamReader(responseStream))
                {
                    xml = streamReader.ReadToEnd();
                }
                XDocument xDoc = XDocument.Parse(xml);

                var xmlExRates = (from xml2 in xDoc.Descendants("Record")
                    select new ExRatesModel
                    {
                        Date = DateHelper.ConvertToDate(xml2.Attribute("Date").Value).Value,
                        Rate = double.Parse(xml2.Element("Rate").Value, CultureInfo.InvariantCulture)
                    }).ToList();

                return xmlExRates;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
