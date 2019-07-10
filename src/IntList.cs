using System;
using System.Collections.Generic;

namespace sharpcode
{
    public class IntList
    {
        static readonly int NIL = -1;
        static readonly int EMPTY = -2;

        int top;
        int count;
        List<int> values;

        public IntList(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException("'capacity' should be > 0");
            }

            this.values = new List<int>();

            Init(capacity);
        }

        public void Init(int capacity)
        {
            top = NIL;
            count = 0;
            values.Capacity = capacity;

            for (int i = 0; i < capacity; i++)
            {
                values.Add(EMPTY);
            }
        }

        public int Capacity
        {
            get
            {
                return values.Count;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return 0 == count;
            }
        }

        public int Top
        {
            get
            {
                return top;
            }
        }

        public int Next(int val)
        {
            if (NIL == val)
            {
                return val;
            }
            else
            {
                return values[val];
            }
        }

        public bool Contains(int val)
        {
            if (val < 0 || val >= Capacity)
            {
                throw new ArgumentOutOfRangeException($"'val' should be in [0;{Capacity})");
            }

            return EMPTY != values[val];
        }

        public void Push(int val)
        {
            if (val < 0 || val >= Capacity)
            {
                throw new ArgumentOutOfRangeException($"'val' should be in [0;{Capacity})");
            }

            if (!Contains(val))
            {
                values[val] = top;
                top = val;

                count += 1;
            }
        }

        public int Pop()
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("'count' should be > 0");
            }

            int val = top;
            top = values[top];
            values[val] = EMPTY;
            count -= 1;

            return val;
        }

        public List<int> PopAll()
        {
            List<int> pops = null;

            PopAll(out pops);

            return pops;
        }

        public void PopAll(out List<int> pops)
        {
            pops = new List<int>(count);

            while (!IsEmpty)
            {
                pops.Add(Pop());
            }
        }

        public void Clear()
        {
            while (!IsEmpty)
            {
                Pop();
            }
        }
    }
}