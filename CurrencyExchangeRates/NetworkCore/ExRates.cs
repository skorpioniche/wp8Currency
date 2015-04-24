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
    public class ExRates
    {
        public ExRatesModel GetRate(DateTime date)
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
                                       where xml2.Element("CharCode").Value == "USD"
                                       select xml2).FirstOrDefault();

            var result = new ExRatesModel
            {
                Rate = Convert.ToInt32(xmlDataElement.Element("Rate").Value),
                Date = date
            };

            return result;
        }

        public List<ExRatesModel> GetRatePeriod(DateTime dateFrom, DateTime dateTo)
        {
            string xml = string.Empty;
            var url = new Uri("http://www.nbrb.by/Services/XmlExRatesDyn.aspx?curId=145&fromDate="
                + DateHelper.ConvertToStringDate(dateFrom) + "&toDate=" + DateHelper.ConvertToStringDate(dateTo));
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
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
                                      Rate = Convert.ToInt32(xml2.Element("Rate").Value)
                                  }).ToList();

            return xmlExRates;
        }
    }
}
