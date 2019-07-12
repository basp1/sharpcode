using System;
using System.Linq;
using System.Collections.Generic;

namespace sharpcode
{
#pragma warning disable CS0659
    public class SegmentTree<T>
#pragma warning restore CS0659
    {
        int capacity;
        T limit;
        Func<T, T, T> selectFunc;
        List<T> values;

        public SegmentTree(T limit, Func<T, T, T> selectFunc, IList<T> values) :
        this(limit, selectFunc, values.Count)
        {
            for (int i = 0; i < values.Count; i++)
            {
                this.values[capacity - 1 + i] = values[i];
            }

            for (int i = capacity - 2; i >= 0; i--)
            {
                this.values[i] = selectFunc(this.values[1 + (i << 1)], this.values[2 + (i << 1)]);
            }
        }

        public SegmentTree(T limit, Func<T, T, T> selectFunc, int dim)
        {
            this.limit = limit;
            this.selectFunc = selectFunc;

            capacity = 1 << (int)Math.Ceiling(Math.Log(dim, 2));
            int cap = capacity << 1;

            values = new List<T>(cap);

            for (int i = 0; i < cap; i++)
            {
                values.Add(limit);
            }
        }

        public int Capacity
        {
            get
            {
                return capacity;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SegmentTree<T>);
        }

        public bool Equals(SegmentTree<T> that)
        {
            if (capacity != that.Capacity)
            {
                return false;
            }

            if (!values.SequenceEqual(that.values))
            {
                return false;
            }

            return true;
        }

        public T this[int index]
        {
            get
            {
                return Range(index, index);
            }

            set
            {
                SetValue(index, value);
            }
        }

        void SetValue(int index, T value)
        {
            if (index >= capacity)
            {
                throw new ArgumentOutOfRangeException("'index' should be < 'capacity'");
            }

            index += (capacity - 1);
            values[index] = value;

            while (index > 0)
            {
                T val = values[index];
                int neighbor;
                if (1 == index % 2)
                {
                    neighbor = index + 1;
                    index = index >> 1;
                }
                else
                {
                    neighbor = index - 1;
                    index = (index >> 1) - 1;
                }

                value = selectFunc(val, values[neighbor]);
                if (!value.Equals(values[index]))
                {
                    values[index] = value;
                }
                else
                {
                    break;
                }
            }
        }

        public T Top
        {
            get
            {
                return values[0];
            }
        }

        public T Range(int left, int right)
        {
            if (left > right)
            {
                throw new ArgumentOutOfRangeException("'left' should be <= 'right'");
            }
            if (left >= capacity)
            {
                throw new ArgumentOutOfRangeException("'left' should be < 'capacity'");
            }
            if (right >= capacity)
            {
                throw new ArgumentOutOfRangeException("'right' should be < 'capacity'");
            }

            left += (capacity - 1);
            right += (capacity - 1);
            T leftValue = limit;
            T rightValue = limit;

            while (left < right)
            {
                if (0 == left % 2)
                {
                    leftValue = selectFunc(leftValue, values[left]);
                }
                left = left >> 1;

                if (1 == right % 2)
                {
                    rightValue = selectFunc(values[right], rightValue);
                }
                right = (right >> 1) - 1;
            }

            if (left == right)
            {
                leftValue = selectFunc(leftValue, values[left]);
            }

            return selectFunc(leftValue, rightValue);
        }
    }
}