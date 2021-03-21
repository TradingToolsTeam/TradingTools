namespace TradingTools.DataLoadService.Services
{
    public interface IInfluxdbService
    {
        void InsertData(string ticker, string filePath);
    }
}