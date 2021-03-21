namespace TradingTools.DataLoadService.Entities.Configuration
{
    public class InfluxdbOptions
    {
        public string Token { get; set; }

        public string BucketName { get; set; }

        public string Organization { get; set; }
    }
}
