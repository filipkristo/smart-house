using SmartHouseWebLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseWebLib.DomainModels
{
    public class TelemetryChartHourUI
    {
        public DateTime Time { get; set; }
        public decimal Temperature { get; set; }
        public decimal Humidity { get; set; }
        public decimal Gas { get; set; }

        public static Expression<Func<TelemetryData, TelemetryChartHourUI>> Select => (d)
            => new TelemetryChartHourUI()
            {
                Gas = d.GasValue,
                Humidity = d.Humidity,
                Temperature = d.Temperature,
                Time = d.CreatedUtc
            };
    }
}
