namespace OrviboController.Common
{
    public class DeviceEventArgs
    {
        private readonly Device _device;

        public DeviceEventArgs(Device device)
        {
            _device = device;
        }

        public Device Device
        {
            get { return _device; }
        }
    }      
}
