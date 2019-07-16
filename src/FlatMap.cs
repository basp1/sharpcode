using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace basp.sharpcode
{
#pragma warning disable CS0659
    public class FlatMap<Key, Value> : IEnumerable<KeyValuePair<Key, Value>>
#pragma warning restore CS0659
    {
        List<Key> keys;
        List<Value> values;
        Func<Key, Key, int> compareFunc;

        public FlatMap(Func<Key, Key, int> compareFunc)
        {
            this.compareFunc = compareFunc;
            keys = new List<Key>();
            values = new List<Value>();
        }

        public bool IsEmpty
        {
            get
            {
                return 0 == keys.Count;
            }
        }

        public int Count
        {
            get
            {
                return keys.Count;
            }
        }

        public int Capacity
        {
            get
            {
                return keys.Capacity;
            }
        }

        public IList<Key> Keys
        {
            get
            {
                return keys;
            }
        }

        public IList<Value> Values
        {
            get
            {
                return values;
            }
        }

        public void Reserve(int capacity)
        {
            keys.Capacity = capacity;
            values.Capacity = capacity;
        }

        public void Clear()
        {
            keys.Clear();
            values.Clear();
        }

        public bool Contains(Key key)
        {
            Value value = default;
            return Find(key, ref value);
        }

        public Value Get(Key key)
        {
            Value value = default;

            if (!Find(key, ref value))
            {
                throw new KeyNotFoundException("'key' not found");
            }

            return value;
        }

        public void Insert(Key key, Value value)
        {
            int index = LowerBound(key);

            if (index >= 0 && index < Count && 0 == compareFunc(key, keys[index]))
            {
                keys[index] = key;
                values[index] = value;
            }
            else
            {
                keys.Insert(index, key);
                values.Insert(index, value);
            }
        }

        public void Remove(Key key)
        {
            int index = LowerBound(key);

            if (index >= 0 && index < Count && 0 == compareFunc(key, keys[index]))
            {
                keys.RemoveAt(index);
                values.RemoveAt(index);
            }
        }

        public Value this[Key key]
        {
            get
            {
                return Get(key);
            }

            set
            {
                Insert(key, value);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FlatMap<Key, Value>);
        }

        public bool Equals(FlatMap<Key, Value> that)
        {
            return keys.SequenceEqual(that.keys) && values.SequenceEqual(that.values);
        }

        bool Find(Key key, ref Value value)
        {
            int index = LowerBound(key);

            if (index >= 0 && index < Count && 0 == compareFunc(key, keys[index]))
            {
                value = values[index];
                return true;
            }

            return false;
        }

        int LowerBound(Key key)
        {
            int n = keys.Count - 1;
            int l = 0;
            int r = n;

            while (l <= r)
            {
                int m = l + (r - l) / 2;
                int cmp = compareFunc(key, keys[m]);
                if (0 == cmp)
                {
                    return m;
                }
                if (cmp < 0)
                {
                    r = m - 1;
                }
                else
                {
                    l = m + 1;
                }
            }

            return l;
        }

        public IEnumerator<KeyValuePair<Key, Value>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return new KeyValuePair<Key, Value>(keys[i], values[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}