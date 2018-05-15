using System;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;

namespace NetworkLib
{
    public class Udp
    {
        private Socket socket;

        private const int maxBufferSize = 1024;

        private static int dataSize = Marshal.SizeOf(new ShipData());

        public Udp(EndPoint localEP)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(localEP);
        }

        public Udp(Socket socket)
        {
            this.socket = socket;
        }

        public Exception Send(ref ShipData data, EndPoint remotePoint)
        {
            ConvertDataToBytes(data, out byte[] buffer);
            try
            {
                socket.SendTo(buffer, remotePoint);
            }
            catch (Exception e)
            {
                return e;
            }
            return null;
        }

        public Exception Receive(ref ShipData data, ref EndPoint remotePoint)
        {
            if (socket.Available > 0)
            {
                byte[] buffer = new byte[maxBufferSize];
                int size = 0;
                try
                {
                    size = socket.ReceiveFrom(buffer, ref remotePoint);
                }
                catch (Exception e)
                {
                    return e;
                }
                ConvertBytesToData(buffer, ref data);
                return null;
            }
            return null;
        }

        private void ConvertDataToBytes<T>(T data, out byte[] bytes)
        {
            int dataSize = Marshal.SizeOf(data);

            bytes = new byte[dataSize];
            IntPtr ptr = Marshal.AllocHGlobal(dataSize);

            Marshal.StructureToPtr(data, ptr, true);
            Marshal.Copy(ptr, bytes, 0, dataSize);
            Marshal.FreeHGlobal(ptr);
        }

        private void ConvertBytesToData<T>(byte[] bytes, ref T data)
        {
            IntPtr ptr = Marshal.AllocHGlobal(dataSize);
            Marshal.Copy(bytes, 0, ptr, dataSize);
            data = (T)Marshal.PtrToStructure(ptr, typeof(T));
        }
    }
}
