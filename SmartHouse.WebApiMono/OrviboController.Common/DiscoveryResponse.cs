using System.Net.NetworkInformation;

namespace OrviboController.Common
{
    public class DiscoveryResponse : Response
    {
        private readonly Device _device;

        public DiscoveryResponse(PhysicalAddress macAddr) : base(macAddr)
        {
            
        }

        public DiscoveryResponse(Device device)
        {
            _device = device;
        }
      
        public Device Device
        {
            get { return _device; }
        }
    }
}
