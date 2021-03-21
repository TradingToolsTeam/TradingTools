using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TradingTools.DataLoadService.Services;

namespace TradingTools.DataLoadService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMainService _mainService;

        public Worker(ILogger<Worker> logger, IMainService mainService)
        {
            _logger = logger;
            _mainService = mainService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //TODO: Añadir scheduler que lance el proceso diariamente, primero es necesario que el proceso valide si un ticker ya ha sido procesado previamente
            // while (!stoppingToken.IsCancellationRequested)
            // {
            //     _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //     await Task.Delay(1000, stoppingToken);
            // }

            _mainService.UpdateStockTimeSeries();
        }
    }
}
