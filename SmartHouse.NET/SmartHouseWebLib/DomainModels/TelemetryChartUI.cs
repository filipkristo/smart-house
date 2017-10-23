using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseWebLib.DomainModels
{
    public class TelemetryChartUI
    {
        public decimal MinTemperature { get; set; }
        public decimal MaxTemperature { get; set; }
        public decimal AverageTemperature { get; set; }

        public decimal MinHumidity { get; set; }
        public decimal MaxHumidity { get; set; }
        public decimal AverageHumidity { get; set; }

        public decimal MinGas { get; set; }
        public decimal MaxGas { get; set; }
        public decimal AverageGas { get; set; }

        public int TotalRows { get; set; }
    }
}
