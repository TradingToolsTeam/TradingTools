using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using Microsoft.Extensions.Options;
using TradingTools.DataLoadService.Entities;
using TradingTools.DataLoadService.Entities.Configuration;

namespace TradingTools.DataLoadService.Services
{
    public class InfluxdbService : IInfluxdbService
    {
        private readonly IOptions<InfluxdbOptions> _influxdbOptions;

        public InfluxdbService(IOptions<InfluxdbOptions> influxdbOptions)
        {
            _influxdbOptions = influxdbOptions;
        }

        public void InsertData(string ticker, string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var list = csv.GetRecords<Intraday>().ToList();

            var client = InfluxDBClientFactory.Create("http://localhost:8086", _influxdbOptions.Value.Token.ToCharArray());
            using var writeApi = client.GetWriteApi();

            foreach (var measurement in list)
            {
                measurement.Time = DateTime.SpecifyKind(measurement.Time, DateTimeKind.Utc);
                measurement.Ticker = ticker;
                writeApi.WriteMeasurement(_influxdbOptions.Value.BucketName, _influxdbOptions.Value.Organization, WritePrecision.Ns, measurement);
            }
        }
    }
}
