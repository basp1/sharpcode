using System;

namespace basp.sharpcode
{
    public class BitSet
    {
        long[] values;
        int count;

        public BitSet(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("'count' should be > 0");
            }

            int n = (int)Math.Ceiling(count / 32d);
            this.count = count;
            values = new long[n];
        }

        public bool this[int index]
        {
            get
            {
                return Get(index);
            }
            set
            {
                Set(index, value);
            }
        }

        public bool Get(int index)
        {
            if (index < 0 || index >= count)
            {
                throw new ArgumentOutOfRangeException($"'index' should be in [0;{count})");
            }

            int i = index >> 5;

            long x = values[i];

            x >>= (index % 32);
            x &= 1;

            return (1 == x);
        }


        public void Set(int index, bool value)
        {
            if (index < 0 || index >= count)
            {
                throw new ArgumentOutOfRangeException($"'index' should be in [0;{count})");
            }

            int i = index >> 5;
            long x = 1 << (index % 32);
            if (value)
            {
                values[i] |= x;
            }
            else
            {
                x = ~x;
                values[i] &= x;
            }
        }

        public long[] Values
        {
            get
            {
                return values;
            }
        }
    }
}
