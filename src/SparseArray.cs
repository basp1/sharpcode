using System;
using System.Collections;
using System.Collections.Generic;

namespace sharpcode
{
#pragma warning disable CS0659
    public class SparseArray<T> : IEnumerable<KeyValuePair<int, T>>
#pragma warning restore CS0659
    {
        int size;
        T @default;
        FlatMap<int, T> items;

        public SparseArray(int size, T @default = default)
        {
            this.size = size;
            this.@default = @default;
            this.items = new FlatMap<int, T>((i, j) => i - j);
        }

        public SparseArray(IList<T> init, T @default = default) : this(init.Count, @default)
        {
            for (int i = 0; i < size; i++)
            {
                Set(i, init[i]);
            }
        }

        public SparseArray(int size, IEnumerable<KeyValuePair<int, T>> init, T @default = default) : this(size, @default)
        {
            foreach (var item in init)
            {
                Set(item.Key, item.Value);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SparseArray<T>);
        }

        public bool Equals(SparseArray<T> that)
        {
            if (size != that.size)
            {
                return false;
            }

            return items.Equals(that.items);
        }

        public void Set(int index, T value)
        {
            if (index < 0 || index >= size)
            {
                throw new IndexOutOfRangeException($"'index' should be in [0; {size})");
            }

            items.Insert(index, value);
        }

        public bool Contains(int index)
        {
            if (index < 0 || index >= size)
            {
                throw new IndexOutOfRangeException($"'index' should be in [0; {size})");
            }

            return items.Contains(index);
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public int Size
        {
            get
            {
                return size;
            }
        }

        public void Reserve(int capacity)
        {
            items.Reserve(capacity);
        }

        public void Clear()
        {
            items.Clear();
        }

        public T Get(int index)
        {
            if (index < 0 || index >= size)
            {
                throw new IndexOutOfRangeException($"'index' should be in [0; {size})");
            }

            if (Contains(index))
            {
                return items.Get(index);
            }
            else
            {
                return @default;
            }
        }

        public void Remove(int index)
        {
            if (index < 0 || index >= size)
            {
                throw new IndexOutOfRangeException($"'index' should be in [0; {size})");
            }

            items.Remove(index);
        }

        public T this[int index]
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

        public IEnumerator<KeyValuePair<int, T>> GetEnumerator()
        {
            foreach (var item in items)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
