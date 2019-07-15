using System;
using System.Collections.Generic;

namespace basp.sharpcode
{
    public class PriorityQueue<T>
    {
        int count;
        List<T> values;
        Func<T, T, T> selectFunc;

        public PriorityQueue(Func<T, T, T> selectFunc, int capacity = 0)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("'capacity' should be >= 0");
            }

            this.selectFunc = selectFunc;

            values = new List<T>(capacity);
            count = 0;
        }

        public void Push(T value)
        {
            int index = count;

            if (values.Count <= index)
            {
                values.Add(value);
            }
            else
            {
                values[index] = value;
            }

            count += 1;

            Promote(index);
        }

        public T Pop()
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("'count' should be > 0");
            }

            T t = Top;

            if (1 == count)
            {
                count = 0;
            }
            else
            {
                T last = values[count - 1];
                values[0] = last;
                count -= 1;

                Demote(0);
            }

            return t;
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        public T Top
        {
            get
            {
                if (count <= 0)
                {
                    throw new ArgumentOutOfRangeException("'count' should be > 0");
                }

                return values[0];
            }
        }

        public int Height
        {
            get
            {
                return 1 + (int)Math.Log(count, 2);
            }
        }

        void Promote(int index)
        {
            if (index < 0 || index >= count)
            {
                throw new ArgumentOutOfRangeException($"'index' should be in [0;{count})");
            }

            if (0 == index)
            {
                return;
            }

            int parent = (index / 2);

            while (index > 0)
            {
                T t = values[index];

                if (!t.Equals(selectFunc(t, values[parent])))
                {
                    break;
                }

                values[index] = values[parent];
                values[parent] = t;

                int Next = parent;
                parent = (index / 2);
                index = Next;
            }
        }

        void Demote(int index)
        {
            if (index < 0 || index >= count)
            {
                throw new ArgumentOutOfRangeException($"'index' should be in [0;{count})");
            }

            if (count == (1 + index))
            {
                return;
            }

            T value = values[index];

            while (index < count)
            {
                int right = (1 + index) * 2;
                T rv = default;
                if (right < count)
                {
                    rv = this.values[right];
                }

                int left = right - 1;
                T lv = default;
                if (left < count)
                {
                    lv = this.values[left];
                }

                int child = -1;
                if (right < count && left < count && lv.Equals(selectFunc(lv, rv)))
                {
                    child = left;
                }
                else if (right < count)
                {
                    child = right;
                }
                else if (left < count)
                {
                    child = left;
                }

                if (child < 0 || value.Equals(selectFunc(value, values[child])))
                {
                    break;
                }
                else
                {
                    values[index] = values[child];
                    values[child] = value;
                    index = child;
                }
            }
        }
    }
}