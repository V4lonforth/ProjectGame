using NetworkLib;
using NetworkLib.Data;
using System;

namespace AndroidGame.Net
{
    class Sender
    {
        private StructConverter converter;

        private ConnectedUdp connectedUdp;

        private byte[] buffer;
        private int currentBufferSize;
        private const int bufferSize = 1024;

        public Sender(ConnectedUdp udp)
        {
            connectedUdp = udp;
            converter = new StructConverter();
            buffer = new byte[bufferSize];
        }

        public void Add<T>(DataType type, T obj) where T : struct
        {
            int size = converter.ConvertStructToBytes(obj, type, out byte[] bytes);
            Array.Copy(bytes, 0, buffer, currentBufferSize, size);
            currentBufferSize += size;
        }
        public void Send()
        {
            byte[] bytes = new byte[currentBufferSize];
            Array.Copy(buffer, bytes, currentBufferSize);
            connectedUdp.Send(bytes);
            currentBufferSize = 0;
        }
    }
}
