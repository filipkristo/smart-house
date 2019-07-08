using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class RabbitMqService : IRabbitMqService, IDisposable
    {
        private readonly IConnection _connection;

        public RabbitMqService()
        {
            var factory = new ConnectionFactory() { HostName = "10.110.166.99", UserName = "XXX", Password = "XXX" };
            _connection = factory.CreateConnection();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public void Hello(string message)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare("hello", "fanout");
                
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "hello",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
            }
        }

        public void SendDashboardData(DashboardData dashboardData)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare("dashboard-data", "fanout");

                var message = JsonConvert.SerializeObject(dashboardData);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "dashboard-data",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
            }
        }

        public void SendNotification(string message)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare("notification", "fanout");
                
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "notification",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
            }
        }

        public void SongChange(PandoraResult pandoraResult)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare("song-change", "fanout");

                var message = JsonConvert.SerializeObject(pandoraResult);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "song-change",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
            }
        }

        public void UpdateTemperature(TelemetryData telemetryData)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare("telemetry-change", "fanout");

                var message = JsonConvert.SerializeObject(telemetryData);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "telemetry-change",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
            }
        }

        public void VoulumeChange(int volume)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare("volume-change", "fanout");

                var message = volume.ToString();
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "volume-change",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
