using System;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace OrviboController.Common
{
    public delegate void DeviceEventHandler(object sender, DeviceEventArgs e);
    public delegate void ResponseEventHandler(object sender, ResponseEventArgs e);

    public class Controller : IDisposable
    {
        private const int UdpPort = 10000;
        private const int RspTimeoutMs = 1000;
        private const int MaxCmdRetries = 5;


        public event DeviceEventHandler OnFoundNewDevice;
        public event ResponseEventHandler OnNewResponse;

        private bool _isListening;
        private UdpListener _udpListener;

        private readonly AutoResetEvent _gotResponse = new AutoResetEvent(false);
        
        private Controller()
        {
            
        }

        public static Controller CreateController(bool autoStart)
        {
            var controller = new Controller();
            if (autoStart)
                controller.StartListening();
            return controller;
        }

        public bool IsListening
        {
            get { return _isListening; }
        }

        public bool StartListening()
        {
            if (_isListening)
                return true;

            StartUdpListener();

            _isListening = true;

            return true;
        }

        public bool CancelListening()
        {
            if (!_isListening)
                return true;

            StopUdpListener();

            _isListening = false;

            return true;
        }

        public bool SendDiscoveryCommand()
        {
            var cmd = Command.CreateDiscoveryCommand();
            _udpListener.SendBroadcast(cmd.Data);

            return true;
        }

        public bool SendCommand(Device device, Command command)
        {
            Debug.WriteLine("Tx: " + command.Type + string.Format(" (0x{0:X})", command.Type));

            _udpListener.Send(command.Data, new IPEndPoint(device.IpAddr, UdpPort));
            return true;
        }

        public bool SendCommandWaitResponse(Device device, Command command)
        {
            int retry = 0;
            bool success;

            _gotResponse.Reset();

            do
            {
                SendCommand(device, command);

                success = _gotResponse.WaitOne(RspTimeoutMs);
            } while (!success && ++retry < MaxCmdRetries);

            return success;
        }

        private bool StartUdpListener()
        {
            _udpListener = new UdpListener(UdpPort);
            _udpListener.OnRxNewData += listener_OnRxNewData;
            _udpListener.StartListening();

            return true;
        }

        private bool StopUdpListener()
        {
            if (_udpListener != null)
                _udpListener.CancelListening();
            return true;
        }

        void listener_OnRxNewData(object sender, IPEndPoint remoteEP, byte[] data)
        {
                var rsp = Response.ParseResponse(data);
       
                if(rsp.Type == EnumResponseType.DiscoveryResponse)
                {
                    if (OnFoundNewDevice != null)
                    {
                        var device = Device.CreateDevice( remoteEP.Address,  ((DiscoveryResponse)rsp).MacAddress );
                        var eventArgs = new DeviceEventArgs(device);
                        try
                        {
                            OnFoundNewDevice(this, eventArgs);
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    if(OnNewResponse != null)
                    {
                        try
                        {
                            OnNewResponse(this, new ResponseEventArgs(rsp));
                        }
                        catch {}
                    }

                    _gotResponse.Set();
                }
        }

        public void Dispose()
        {
            if(IsListening)
            {
                _udpListener.Dispose();
                _udpListener.OnRxNewData -= listener_OnRxNewData;
            }            
        }
    }
}
