using System;
using System.Net;
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

        public void SendBroadcast(byte[] data)
        {
            _client.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, _port));
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
