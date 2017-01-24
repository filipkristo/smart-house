using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace OrviboController.Common
{
    public delegate void DataHandler(object sender, IPEndPoint remoteEP, byte[] data);

    public class UdpListener : IDisposable
    {
        private readonly int _port;
        private IPEndPoint _ep;
        private UdpClient _client;
        private bool _isListening;

        public event DataHandler OnRxNewData;

        public UdpListener(int port)
        {
            _port = port;
        }

        public UdpClient Client
        {
            get { return _client; }
        }

        public bool StartListening()
        {
            if (_isListening)
                return true;
            
            _ep = new IPEndPoint(IPAddress.Any, _port);
            _client = new UdpClient(_ep);
            _client.EnableBroadcast = true;
            _client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _client.BeginReceive(ReceiveDataCB, null);

            _isListening = true;
            return true;
        }

        public bool CancelListening()
        {
            if (!_isListening)
                return true;

            _isListening = false;
            _client.Close();            

            return true;
        }

        public void Send(byte[] data, IPEndPoint ep)
        {
            _client.Send(data, data.Length, ep);
        }

        /// <summary>
        /// Check function DisplayDirectedBroadcastAddresses for IP address
        /// </summary>
        /// <param name="data"></param>
        public void SendBroadcast(byte[] data)
        {
            _client.Send(data, data.Length, new IPEndPoint(IPAddress.Parse("10.110.167.255"), _port));
        }

        public void DisplayDirectedBroadcastAddresses()
        {

            foreach (var iface in NetworkInterface.GetAllNetworkInterfaces()
                     .Where(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback))
            {
                Console.WriteLine(iface.Description);
                foreach (var ucastInfo in iface.GetIPProperties().UnicastAddresses
                         .Where(c => !c.Address.IsIPv6LinkLocal))
                {
                    Console.WriteLine("\tIP       : {0}", ucastInfo.Address);
                    Console.WriteLine("\tSubnet   : {0}", ucastInfo.IPv4Mask);
                    byte[] ipAdressBytes = ucastInfo.Address.GetAddressBytes();
                    byte[] subnetMaskBytes = ucastInfo.IPv4Mask.GetAddressBytes();

                    if (ipAdressBytes.Length != subnetMaskBytes.Length) continue;

                    var broadcast = new byte[ipAdressBytes.Length];
                    for (int i = 0; i < broadcast.Length; i++)
                    {
                        broadcast[i] = (byte)(ipAdressBytes[i] | ~(subnetMaskBytes[i]));
                    }
                    Console.WriteLine("\tBroadcast: {0}", new IPAddress(broadcast).ToString());
                }
            }

        }

        private void ReceiveDataCB(IAsyncResult ar)
        {
            if (!_isListening)
                return;

            IPEndPoint remoteEP = null;

            var data = _client.EndReceive(ar, ref remoteEP);
            
            if (OnRxNewData != null)
            {
                try
                {
                    OnRxNewData(this, remoteEP, data);
                }
                catch (Exception)
                {
                }
            }

            if(_isListening)
                _client.BeginReceive(ReceiveDataCB, null);
        }

        public void Dispose()
        {                        
            CancelListening();
        }
    }
}
