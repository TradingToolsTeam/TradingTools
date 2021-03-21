using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Options;
using RestSharp;
using TradingTools.DataLoadService.Entities.Configuration;

namespace TradingTools.DataLoadService.Services
{
    public class StockDataService : IStockDataService
    {
        private readonly IOptions<StockDataOptions> _stockDataOptions;

        private List<string> _slices = new List<string>
        {
            "year1month1",
            "year1month2",
            "year1month3",
            "year1month4",
            "year1month5",
            "year1month6",
            "year1month7",
            "year1month8",
            "year1month9",
            "year1month10",
            "year1month11",
            "year1month12",
            "year2month1",
            "year2month2",
            "year2month3",
            "year2month4",
            "year2month5",
            "year2month6",
            "year2month7",
            "year2month8",
            "year2month9",
            "year2month10",
            "year2month11",
            "year2month12",
        };

        public StockDataService(IOptions<StockDataOptions> stockDataOptions)
        {
            _stockDataOptions = stockDataOptions;
        }

        public IEnumerable<string> GetStockDataFiles(string ticker)
        {
            foreach (var slice in _slices)
            {
                yield return GetStockDataFiles(ticker, slice);

                //TODO: Se esparan 15 segundos por la limitación del API de AlphaVantage, cambiar esto por una comprobación de la respuesta y una añadir una espera solo si falla
                Thread.Sleep(15_000);
            }
        }

        private string GetStockDataFiles(string ticker, string slice)
        {
            var path = Path.GetTempFileName();

            var client = new RestClient("https://www.alphavantage.co");
            var request = new RestRequest("query", Method.GET);
            request.AddParameter("function", "TIME_SERIES_INTRADAY_EXTENDED");
            request.AddParameter("symbol", ticker);
            request.AddParameter("interval", "1min");
            request.AddParameter("slice", slice);
            request.AddParameter("apikey", _stockDataOptions.Value.ApiKey);

            var response = client.Execute(request);

            File.WriteAllText(path, response.Content);

            return path;
        }
    }
}
