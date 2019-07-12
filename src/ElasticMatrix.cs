using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace sharpcode
{
#pragma warning disable CS0659
    public class ElasticMatrix<T>
#pragma warning restore CS0659
    {
        static readonly int NULL = -1;

        List<int> rows;
        List<int> columns;
        List<int> next;
        List<T> values;
        List<int> count;

        int free = NULL;
        List<int> last;

        public ElasticMatrix(int rows, int columns, int capacity = 0)
        {
            this.Count = 0;
            this.Rows = rows;
            this.Columns = columns;
            this.free = NULL;

            this.rows = new List<int>(new int[Rows]);
            this.count = new List<int>(new int[Rows]);
            this.columns = new List<int>(capacity);
            this.next = new List<int>(capacity);
            this.values = new List<T>(capacity);
            this.last = new List<int>(new int[Rows]);

            for (int i = 0; i < Rows; i++)
            {
                this.rows[i] = NULL;
                this.last[i] = NULL;
            }
        }

        public ElasticMatrix(ElasticMatrix<T> that) : this(that.Rows, that.Columns, that.Capacity)
        {
            Count = that.Count;
            free = that.free;
            this.rows = new List<int>(that.rows);
            this.count = new List<int>(that.count);
            this.last = new List<int>(that.last);
            this.columns = new List<int>(that.columns);
            this.next = new List<int>(that.next);
            this.values = new List<T>(that.values);
        }

        public int Rows { get; private set; }

        public int Columns { get; set; }

        public int Count { get; private set; }

        public int Capacity { get { return values.Capacity; } }

        public override bool Equals(object obj)
        {
            if (!(obj is ElasticMatrix<T>))
            {
                return false;
            }

            var that = (ElasticMatrix<T>)obj;

            if (Count != that.Count)
            {
                return false;
            }
            if (Rows != that.Rows)
            {
                return false;
            }
            if (Columns != that.Columns)
            {
                return false;
            }

            if (!count.SequenceEqual(that.count))
            {
                return false;
            }

            for (int i = 0; i < Rows; i++)
            {
                for (int j = rows[i], k = that.rows[i]; NULL != j && NULL != k; j = next[j], k = that.next[k])
                {
                    if (columns[j] != that.columns[k])
                    {
                        return false;
                    }
                    if (!values[j].Equals(that.values[k]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public T this[int row, int column]
        {
            get
            {
                return Get(row, column);
            }

            set
            {
                Set(row, column, value);
            }
        }

        public T Get(int row, int column)
        {
            if (column < 0 || column >= Columns)
            {
                throw new ArgumentOutOfRangeException($"'column' should be in [0;{Columns})");
            }
            if (row < 0 || row >= Rows)
            {
                throw new ArgumentOutOfRangeException($"'row' should be in [0;{Rows})");
            }

            for (int j = rows[row]; NULL != j; j = next[j])
            {
                if (column == columns[j])
                {
                    return values[j];
                }
            }

            return default;
        }

        public void Set(int row, int column, T value)
        {
            if (column < 0 || column >= Columns)
            {
                throw new ArgumentOutOfRangeException($"'column' should be in [0;{Columns})");
            }
            if (row < 0 || row >= Rows)
            {
                throw new ArgumentOutOfRangeException($"'row' should be in [0;{Rows})");
            }

            for (int j = rows[row]; NULL != j; j = next[j])
            {
                if (column == columns[j])
                {
                    values[j] = value;
                    return;
                }
            }

            Add(row, column, value);
        }

        public int Add(int row, int column, T value)
        {
            int n;
            if (free >= 0)
            {
                n = free;
                values[free] = value;
                columns[free] = column;
                free = next[free];
            }
            else
            {
                n = Count;
                next.Add(NULL);
                values.Add(value);
                columns.Add(column);
            }

            if (NULL == rows[row])
            {
                rows[row] = n;
            }
            else
            {
                next[last[row]] = n;
            }

            last[row] = n;
            next[n] = -1;
            count[row] += 1;
            Count += 1;

            return n;
        }

        public void Remove(int row, int column)
        {
            int k = NULL;
            int p = NULL;
            for (int j = rows[row]; NULL != j; j = next[j])
            {
                if (column == columns[j])
                {
                    k = j;
                    break;
                }
                p = j;
            }

            if (NULL == k)
            {
                return;
            }

            if (rows[row] == k)
            {
                rows[row] = next[k];
                next[k] = free;
                free = k;
                if (last[row] == k)
                {
                    last[row] = rows[row];
                }
            }
            else
            {
                next[p] = next[k];
                next[k] = free;
                free = k;
                if (last[row] == k)
                {
                    last[row] = p;
                }
            }

            count[row] -= 1;
            Count -= 1;
        }

        public int RemoveRow(int row)
        {
            if (0 == count[row])
            {
                return 0;
            }

            next[last[row]] = free;
            free = rows[row];

            rows[row] = NULL;
            last[row] = NULL;
            Count -= count[row];
            int k = count[row];
            count[row] = 0;

            return k;
        }

        public void AddRow()
        {
            rows.Add(NULL);
            last.Add(NULL);
            count.Add(0);

            Rows += 1;
        }

        public bool Contains(int row, int column)
        {
            if (row < 0 || row >= Rows || column < 0 || column >= Columns)
            {
                return false;
            }

            for (int j = rows[row]; NULL != j; j = next[j])
            {
                if (column == columns[j])
                {
                    return true;
                }
            }

            return false;
        }

        public ElasticMatrix<T> Transpose()
        {
            var t = new ElasticMatrix<T>(Columns, Rows, Count);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = rows[i]; NULL != j; j = next[j])
                {
                    t.Add(columns[j], i, values[j]);
                }
            }

            return t;
        }

        public int RowCount(int row)
        {
            if (row < 0 || row >= Rows)
            {
                throw new ArgumentOutOfRangeException($"'row' should be in [0;{Rows})");
            }

            return count[row];
        }

        public void Sorting()
        {
            int n = count.Max();
            int[] cols = new int[n];
            int[] indices = new int[n];
            for (int i = 0; i < Rows; i++)
            {
                if (count[i] < 2) continue;

                for (int j = rows[i], k = 0; NULL != j; j = next[j], k++)
                {
                    cols[k] = this.columns[j];
                    indices[k] = j;
                }

                Array.Sort(cols, indices, 0, count[i]);

                for (int j = 1; j < count[i]; j++)
                {
                    next[indices[j - 1]] = indices[j];
                }

                rows[i] = indices[0];
                last[i] = indices[count[i] - 1];
                next[indices[count[i] - 1]] = NULL;
            }
        }

        public void ForRow(int row, Action<int, T> func)
        {
            for (int i = rows[row]; NULL != i; i = next[i])
            {
                func(columns[i], values[i]);
            }
        }

        public SparseArray<T> GetRow(int row)
        {
            var array = new SparseArray<T>(Columns);

            for (int j = rows[row]; NULL != j; j = next[j])
            {
                array[columns[j]] = values[j];
            }

            return array;
        }
    }
}