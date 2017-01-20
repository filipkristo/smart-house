using System.IO;

namespace OrviboController.Common
{
    public enum EnumCommandType
    {
        Unknown = 0x00,
        Discovery,
        Subscription,
        On,
        Off,
    }

    public class Command
    {
        public const byte MagicKey1 = 0x68;
        public const byte MagicKey2 = 0x64;

        public EnumCommandType Type { get; private set; }

        public byte[] Data { get; private set; }

        public bool PowerState { get; set; }

        public static Command CreatePowerControlCommand(Device device, bool isOn)
        {
            var ms = new MemoryStream();

            // Magic Key
            ms.WriteByte(MagicKey1);
            ms.WriteByte(MagicKey2);

            // Length
            ms.WriteByte(0x00);
            ms.WriteByte(0x17);

            // Command ID
            ms.WriteByte(0x64);
            ms.WriteByte(0x63);

            // MAC Address
            var macBytes = device.MacAddr.GetAddressBytes();
            ms.WriteByte(macBytes[0]);
            ms.WriteByte(macBytes[1]);
            ms.WriteByte(macBytes[2]);
            ms.WriteByte(macBytes[3]);
            ms.WriteByte(macBytes[4]);
            ms.WriteByte(macBytes[5]);

            // MAC padding (spaces)
            ms.WriteByte(0x20);
            ms.WriteByte(0x20);
            ms.WriteByte(0x20);
            ms.WriteByte(0x20);
            ms.WriteByte(0x20);
            ms.WriteByte(0x20);

            // Unknown
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);
            ms.WriteByte(0x00);

            // Requested power state
            ms.WriteByte(isOn ? (byte)0x01 : (byte)0x00);

            ms.Close();
            var data = ms.ToArray();

            var cmd = new Command { Type = isOn ? EnumCommandType.On : EnumCommandType.Off, Data = data };

            return cmd;
        }

        public static Command CreateOnCommand(Device device)
        {
            return CreatePowerControlCommand(device, true);
        }

        public static Command CreateOffCommand(Device device)
        {
            return CreatePowerControlCommand(device, false);
        }

        public static Command CreateSubscribeCommand(Device device)
        {
            var ms = new MemoryStream();

            // Magic Key
            ms.WriteByte(MagicKey1);
            ms.WriteByte(MagicKey2);

            // Length
            ms.WriteByte(0x00);
            ms.WriteByte(0x1E); 

            // Command ID
            ms.WriteByte(0x63);
            ms.WriteByte(0x6C);

            // MAC Address
            var macBytes = device.MacAddr.GetAddressBytes();
            ms.WriteByte(macBytes[0]);
            ms.WriteByte(macBytes[1]);
            ms.WriteByte(macBytes[2]);
            ms.WriteByte(macBytes[3]);
            ms.WriteByte(macBytes[4]);
            ms.WriteByte(macBytes[5]);

            // MAC padding (spaces)
            ms.WriteByte(0x20);
            ms.WriteByte(0x20);
            ms.WriteByte(0x20);
            ms.WriteByte(0x20);
            ms.WriteByte(0x20);
            ms.WriteByte(0x20);

            // MAC Address (little endian)
            ms.WriteByte(macBytes[5]);
            ms.WriteByte(macBytes[4]);
            ms.WriteByte(macBytes[3]);
            ms.WriteByte(macBytes[2]);
            ms.WriteByte(macBytes[1]);
            ms.WriteByte(macBytes[0]);
    
            // More padding ?
            ms.WriteByte(0x20);
            ms.WriteByte(0x20);
            ms.WriteByte(0x20);
            ms.WriteByte(0x20);
            ms.WriteByte(0x20);
            ms.WriteByte(0x20);

            ms.Close();
            var data = ms.ToArray();

            var cmd = new Command { Type = EnumCommandType.Subscription, Data = data };

            return cmd;
        }

        public static Command CreateDiscoveryCommand()
        {
            //  68 64 00 06 71 61
            var data = new byte[] {MagicKey1, MagicKey2, 0x00, 0x06, 0x71, 0x61};

            var cmd = new Command {Type = EnumCommandType.Discovery, Data = data};

            return cmd;
        }
    }
}
