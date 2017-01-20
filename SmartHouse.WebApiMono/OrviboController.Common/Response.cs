using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;

namespace OrviboController.Common
{
    public enum EnumResponseType
    {
        UnknownResponse = 0x00,
        DiscoveryResponse,
        SubscriptionResponse,
    }

    enum EnumDeviceCode
    {
        Discover= 0x7161,
        SubscriptionResponse = 0x636C,
        PowerOn = 0x6463, // not sure why we get this
        PowerOnResponse = 0x7366,
    }

    public class Response
    {
        private readonly PhysicalAddress _macAddr;

        internal Response()
        {
            
        }

        internal Response(PhysicalAddress macAddr)
        {
            _macAddr = macAddr;
        }

        public PhysicalAddress MacAddress
        {
            get { return _macAddr; }
        }

        public EnumResponseType Type { get; set; }

        public byte[] Data { get; set; }

        public static Response ParseResponse(byte[] data)
        {
            Response rsp = null;
            var macBytes = new byte[6];

            var ms = new MemoryStream(data);

            var magicByte1 = ms.ReadByte();
            var magicByte2 = ms.ReadByte();

            if (magicByte1 != Command.MagicKey1)
                throw new IOException("Invalid magic");
            if (magicByte2 != Command.MagicKey2)
                throw new IOException("Invalid magic");

            var lengthHigh = ms.ReadByte();
            var lengthLow = ms.ReadByte();
            var length = ((int)lengthHigh) * 0x100 + lengthLow;

            var cmdIdHigh = ms.ReadByte();
            var cmdIdLow = ms.ReadByte();
            var cmdId = (EnumDeviceCode) (cmdIdHigh * 0x100 + cmdIdLow);

            Debug.WriteLine("Rx: " + cmdId + string.Format(" (0x{0:X})", cmdId) + ", Length: " + length);

            switch (cmdId)
            {
                case EnumDeviceCode.Discover :

                    if (length == 0x2A)
                    {
                        // Drop padding byte
                        ms.ReadByte();
                        
                        // Get mac address
                        ms.Read(macBytes, 0, 6);
                        var srcMacAddr = new PhysicalAddress(macBytes);

                        rsp = new DiscoveryResponse(srcMacAddr);

                        ms.Close();
                        rsp.Type = EnumResponseType.DiscoveryResponse;

                        return rsp;                    
                    }

                    return new UnhandledeResponse();

                case EnumDeviceCode.SubscriptionResponse: 
                case EnumDeviceCode.PowerOnResponse: 

                    // Get mac address
                    macBytes = new byte[6];
                    ms.Read(macBytes, 0, 6);
                    // Get padding
                    var dummy = new byte[6];
                    ms.Read(dummy, 0, 6);

                    int powerState = -1;

                    switch (cmdId)
                    {
                        case EnumDeviceCode.SubscriptionResponse:

                            // Get unknown
                            ms.Read(dummy, 0, 5);
                            // Get power state
                            powerState = ms.ReadByte();

                            rsp = new SubscriptionResponse();
                            ((SubscriptionResponse)rsp).PowerState = (powerState != 0);
                            break;
                        case EnumDeviceCode.PowerOnResponse:

                            // Get unknown
                            ms.Read(dummy, 0, 4);
                            // Get power state
                            powerState = ms.ReadByte();

                            rsp = new PowerControlResponse();
                            ((PowerControlResponse)rsp).PowerState = (powerState != 0);
                            break;
                    }

                    ms.Close();
                    rsp.Type = EnumResponseType.SubscriptionResponse;
                    rsp.Data = data;

                    return rsp;                    

                default:
                    return new UnhandledeResponse();
            }
        }

    }
}
