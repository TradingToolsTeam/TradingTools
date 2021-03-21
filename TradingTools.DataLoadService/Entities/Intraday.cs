using System;
using CsvHelper.Configuration.Attributes;
using InfluxDB.Client.Core;

namespace TradingTools.DataLoadService.Entities
{
    [Measurement("intraday")]
    internal class Intraday
    {
        [Column(IsTimestamp = true)]
        [Name("time")]
        public DateTime Time { get; set; }

        [Column("ticker", IsTag = true)]
        [Optional]
        public string Ticker { get; set; }
        [Column("open")]
        [Name("open")]
        public float Open { get; set; }

        [Column("high")]
        [Name("high")]
        public float High { get; set; }

        [Column("low")]
        [Name("low")]
        public float Low { get; set; }

        [Column("close")]
        [Name("close")]
        public float Close { get; set; }

        [Column("volume")]
        [Name("volume")]
        public float Volume { get; set; }
    }
}