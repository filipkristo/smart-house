using SmartHouseWebLib.DomainModels;
using SmartHouseWebLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseWebLib.DomainService.Interface
{
    public interface ITelemetryDataService
    {
        Task<IEnumerable<TelemetryData>> GetAllAsync();
        Task<TelemetryData> GetAsync(int Id);
        Task<TelemetryChartUI> GetChartData(string userId);
        Task<IEnumerable<TelemetryChartHourUI>> GetChartLastHoutData(string userId);
        Task<int> Insert(TelemetryData model);
        Task<int> Update(TelemetryData model);
        Task<int> Delete(TelemetryData model);
        Task<TelemetryData> GetLastAsync(string userId);
    }
}
