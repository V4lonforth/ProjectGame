using System;
using System.Runtime.InteropServices;

namespace NetworkLib.Data
{
    public class StructConverter
    {
        public int ConvertStructsToBytes<T>(T[] obj, DataType type, out byte[] bytes) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T)) + sizeof(byte);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            bytes = new byte[size * obj.Length];
            for (int i = 0; i < obj.Length; i++)
            {
                bytes[i * size] = (byte)type;
                Marshal.StructureToPtr(obj[i], ptr, true);
                Marshal.Copy(ptr, bytes, i * size + sizeof(byte), size - sizeof(byte));
            }
            Marshal.FreeHGlobal(ptr);
            return size * obj.Length;
        }
        public int ConvertStructToBytes<T>(T obj, DataType type, out byte[] bytes) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            bytes = new byte[size + sizeof(byte)];
            bytes[0] = (byte)type;
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, bytes, sizeof(byte), size);
            Marshal.FreeHGlobal(ptr);
            return size + sizeof(byte);
        }

        public int ConvertBytesToStruct<T>(byte[] bytes, int startIndex, out T obj) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, startIndex, ptr, size);
            obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
            return size;
        }

        public DataType GetDataType(byte[] bytes, int index)
        {
            return (DataType)bytes[index];
        }
    }
}
