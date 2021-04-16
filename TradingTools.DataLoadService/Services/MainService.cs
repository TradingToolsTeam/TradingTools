using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TradingTools.DataLoadService.Entities.Configuration;

namespace TradingTools.DataLoadService.Services
{
    public class MainService : IMainService
    {
        private readonly IStockDataService _stockDataService;
        private readonly IInfluxdbService _influxdbService;
        private readonly IOptions<MainOptions> _mainOptions;
        private readonly ILogger<MainService> _logger;

        public MainService(IStockDataService stockDataService, IInfluxdbService influxdbService, IOptions<MainOptions> mainOptions, ILogger<MainService> logger)
        {
            _stockDataService = stockDataService;
            _influxdbService = influxdbService;
            _mainOptions = mainOptions;
            _logger = logger;
        }

        public void UpdateStockTimeSeries()
        {
            foreach (var ticker in _mainOptions.Value.Tickers.Split(';'))
            {
                UpdateTickerTimeSeries(ticker);
            }
        }

        private void UpdateTickerTimeSeries(string ticker)
        {
            var filePaths = _stockDataService.GetStockDataFiles(ticker);

            foreach (var filePath in filePaths)
            {
                _influxdbService.InsertData(ticker, filePath);

                _logger.LogInformation($"Se han insertado en InfluxDB los datos del fichero {filePath} para el ticker {ticker}");
            }
        }
    }
}
