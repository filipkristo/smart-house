using OrviboController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHouse.Lib
{
    public class OrvibioService : IOrvibioService, IDisposable
    {
        private Controller _controller;
        private Device _device;

        private AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        public OrvibioService()
        {
            SetupController();
            DoDiscovery();   
        }

        public Result TurnOn()
        {
            var Ok = _autoResetEvent.WaitOne(10000);

            if(Ok)
            {
                var result = DoSet(_device.MacAddr, _device.IpAddr, true);

                return new Result()
                {
                    ErrorCode = 0,
                    Ok = true,
                    Message = $"Turning on socket. IP:{_device.IpAddr}, Mac: {_device.MacAddr}, Success: {result}"
                };
            }
            else
            {
                return new Result()
                {
                    ErrorCode = 1,
                    Ok = false,
                    Message = "Didn't find device"
                };
            }                
        }

        public Result TurnOff()
        {
            var Ok = _autoResetEvent.WaitOne(10000);

            if (Ok)
            {
                var result = DoSet(_device.MacAddr, _device.IpAddr, false);

                return new Result()
                {
                    ErrorCode = 0,
                    Ok = true,
                    Message = $"Turning on socket. IP:{_device.IpAddr}, Mac: {_device.MacAddr}, Success: {result}"
                };
            }                
            else
            {
                return new Result()
                {
                    ErrorCode = 1,
                    Ok = false,
                    Message = "Didn't find device"
                };
            }
        }
        
        private bool SetupController()
        {
            _controller = Controller.CreateController(true);
            _controller.OnFoundNewDevice += _controller_OnFoundNewDevice;
            _controller.OnNewResponse += _controller_OnNewResponse;

            return true;
        }

        private bool DoSet(PhysicalAddress macAddress, IPAddress ipAddress, bool isOn)
        {
            var device = Device.CreateDevice(ipAddress, macAddress);
            var subscription = Command.CreateSubscribeCommand(device);

            // Subscribe before we can do anything
            var success = _controller.SendCommandWaitResponse(device, subscription);

            if (success)
            {
                // Handle the power control
                var control = Command.CreatePowerControlCommand(device, isOn);
                success = _controller.SendCommandWaitResponse(device, control);
            }

            return success;
        }

        private bool DoDiscovery()
        {
            if (!_controller.IsListening)
                _controller.StartListening();            

            _controller.SendDiscoveryCommand();            
            return true;
        }

        private void _controller_OnFoundNewDevice(object sender, DeviceEventArgs e)
        {
            _device = e.Device;
            Logger.LogInfoMessage($"Found Device with MAC Address: {_device.MacAddr}, IP Address: {_device.IpAddr}");

            _autoResetEvent.Set();
        }

        private void _controller_OnNewResponse(object sender, ResponseEventArgs e)
        {
            var rsp = e.Response;
            if (rsp is PowerControlResponse)
            {
            }
            else if (rsp is SubscriptionResponse)
            {
                Logger.LogInfoMessage($"Power State: {((SubscriptionResponse)rsp).PowerState}");                
            }
        }

        public void Dispose()
        {
            _controller.OnFoundNewDevice -= _controller_OnFoundNewDevice;
            _controller.OnNewResponse -= _controller_OnNewResponse;

            _controller.Dispose();
        }
    }
}
