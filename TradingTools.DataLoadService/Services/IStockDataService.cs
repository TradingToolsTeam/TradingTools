using System.Collections.Generic;

namespace TradingTools.DataLoadService.Services
{
    public interface IStockDataService
    {
        IEnumerable<string> GetStockDataFiles(string ticker);
    }
}