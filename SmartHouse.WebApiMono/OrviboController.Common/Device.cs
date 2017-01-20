using System.Net;
using System.Net.NetworkInformation;

namespace OrviboController.Common
{
    public class Device
    {
        private readonly IPAddress _ipAddr;
        private readonly PhysicalAddress _macAddr;

        private Device(string ipAddr, string macAddr)
        {
            _ipAddr = IPAddress.Parse(ipAddr);
            _macAddr = PhysicalAddress.Parse(macAddr);
        }

        private Device(IPAddress ipAddr, PhysicalAddress macAddr)
        {
            _ipAddr = ipAddr;
            _macAddr = macAddr;
        }

        public static Device CreateDevice(string ipAddr, string macAddr)
        {
            return new Device(ipAddr, macAddr);          
        }

        public static Device CreateDevice(IPAddress ipAddr, PhysicalAddress macAddr)
        {
            return new Device(ipAddr, macAddr);
        }

        public IPAddress IpAddr
        {
            get { return _ipAddr; }
        }

        public PhysicalAddress MacAddr
        {
            get { return _macAddr; }
        }

        public override string ToString()
        {
            return MacAddr + " " + IpAddr;
        }
    }
}
