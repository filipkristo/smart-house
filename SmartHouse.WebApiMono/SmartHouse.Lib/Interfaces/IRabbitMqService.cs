using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public interface IRabbitMqService
    {
        void SendDashboardData(DashboardData dashboardData);

        void SendNotification(string message);

        void VoulumeChange(int volume);

        void SongChange(PandoraResult pandoraResult);

        void UpdateTemperature(TelemetryData telemetryData);

        void Hello(string message);
    }
}
