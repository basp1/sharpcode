using System;
using System.Collections.Generic;
using System.Linq;

namespace basp.sharpcode
{
    public class ByteBuffer
    {
        byte[] data;

        int local;
        int capacity;

        public ByteBuffer(int capacity)
        {
            this.capacity = capacity;
            data = new byte[capacity];
            local = 0;

            Reserve(capacity);
        }

        public int Position
        {
            get
            {
                return local;
            }
            set
            {
                Reserve(value);
                local = value;
            }
        }

        public void Flip()
        {
            Position = 0;
        }

        public byte[] ToArray()
        {
            return data.Take(capacity).ToArray();
        }

        public void Put(byte value)
        {
            int i = Position;
            Position = i + sizeof(byte);
            Put(i, value);
        }

        public void PutInt(uint value)
        {
            int i = Position;
            Position = i + sizeof(uint);
            PutInt(i, value);
        }

        public void PutLong(ulong value)
        {
            int i = local;
            Position = i + sizeof(ulong);
            PutLong(i, value);
        }

        void Put(int i, byte value)
        {
            if (i < 0 || i >= capacity)
            {
                throw new ArgumentOutOfRangeException($"'i' should be in [0;{capacity})");
            }

            data[i] = value;
        }

        void PutInt(int i, uint value)
        {
            if (i < 0 || i >= capacity)
            {
                throw new ArgumentOutOfRangeException($"'i' should be in [0;{capacity})");
            }
            for (int j = 0; j < sizeof(uint); j++)
            {
                data[i + j] = (byte)(value & 0xff);
                value >>= 8;
            }
        }

        void PutLong(int i, ulong value)
        {
            if (i < 0 || i >= capacity)
            {
                throw new ArgumentOutOfRangeException($"'i' should be in [0;{capacity})");
            }
            for (int j = 0; j < sizeof(ulong); j++)
            {
                data[i + j] = (byte)(value & 0xff);
                value >>= 8;
            }
        }

        public byte Get()
        {
            int i = Position;
            Position = i + sizeof(byte);
            return Get(i);
        }

        public uint GetInt()
        {
            int i = Position;
            Position = i + sizeof(uint);
            return GetInt(i);
        }

        public ulong GetLong()
        {
            int i = Position;
            Position = i + sizeof(ulong);
            return GetLong(i);
        }

        public byte Get(int i)
        {
            if (i < 0 || i >= capacity)
            {
                throw new ArgumentOutOfRangeException($"'i' should be in [0;{capacity})");
            }
            return data[i];
        }

        public uint GetInt(int i)
        {
            if (i < 0 || i >= capacity)
            {
                throw new ArgumentOutOfRangeException($"'i' should be in [0;{capacity})");
            }

            uint value = 0;
            for (int j = 0; j < sizeof(uint); j++)
            {
                value |= (uint)data[i + j] << (8 * j);
            }
            return value;
        }

        public ulong GetLong(int i)
        {
            if (i < 0 || i >= capacity)
            {
                throw new ArgumentOutOfRangeException($"'i' should be in [0;{capacity})");
            }

            ulong value = 0;
            for (int j = 0; j < sizeof(ulong); j++)
            {
                value |= ((ulong)data[i + j] << (8 * j));
            }
            return value;
        }

        public void Reserve(int newCapacity)
        {
            // Use a long to prevent overflows
            long arrayLength = data.Length;

            if (newCapacity > arrayLength)
            {
                long newLength = Math.Max(8, arrayLength * 2);
                while (newCapacity > newLength)
                {
                    newLength <<= 1;
                }

                if (newLength > Int32.MaxValue)
                {
                    newLength = Int32.MaxValue;
                }

                if (data.Length < newLength)
                {
                    var tmp = new byte[newLength];
                    Array.Copy(data, tmp, capacity);
                    data = tmp;
                }
            }

            capacity = newCapacity;
        }
    }
}