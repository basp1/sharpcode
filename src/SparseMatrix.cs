using System;
using System.Linq;
using System.Collections.Generic;

namespace basp.sharpcode
{
#pragma warning disable CS0659
    public class SparseMatrix<T>
#pragma warning restore CS0659
    {
        int[] columns;
        int[] columnsRows;
        T[] values;

        int[] rows;
        int[] rowsColumns;
        int[] positions;

        SparseMatrix(int rows, int columns, int capacity)
        {
            Count = 0;
            Rows = rows;
            Columns = columns;

            this.columns = new int[Columns + 1];
            this.columnsRows = new int[capacity];
            this.values = new T[capacity];

            this.rows = new int[Rows + 1];
            this.rowsColumns = new int[capacity];
            this.positions = new int[capacity];
        }

        public SparseMatrix(SparseMatrix<T> that) : this(that.Rows, that.Columns, that.Count)
        {
            Count = that.Count;

            Array.Copy(that.columns, columns, that.Columns + 1);
            Array.Copy(that.columnsRows, columnsRows, that.Count);
            Array.Copy(that.values, values, that.Count);

            Array.Copy(that.rows, rows, that.Rows + 1);
            Array.Copy(that.rowsColumns, rowsColumns, that.Count);
            Array.Copy(that.positions, positions, that.Count);
        }

        public SparseMatrix(ElasticMatrix<T> that) : this(that.Rows, that.Columns, that.Count)
        {
            for (int i = 0, j = 0; i < Rows; i++)
            {
                that.ForRow(i, (col, x) => rowsColumns[j++] = col);
                rows[i + 1] = j;
            }

            that = that.Transpose();

            int[] index = new int[Rows];
            for (int i = 0; i < index.Length; i++)
            {
                index[i] = rows[i];
            }

            for (int j = 0, i = 0; j < Columns; j++)
            {
                that.ForRow(j, (col, x) =>
                {
                    columnsRows[i] = col;
                    values[i] = x;

                    rowsColumns[index[col]] = j;
                    positions[index[col]] = i;

                    index[col]++;
                    i++;
                });
                columns[j + 1] = i;
            }

            Count = that.Count;

            Sorting();
        }

        public SparseMatrix(int m, int n, int[] rows, int[] columns, T[] values) : this(m, n, values.Length)
        {
            Count = values.Length;
            Array.Copy(rows, this.rows, m + 1);

            for (int i = 0; i < m; i++)
            {
                for (int j = rows[i]; j < rows[i + 1]; j++)
                {
                    this.columns[columns[j] + 1]++;
                }
            }

            for (int j = 1; j < (1 + n); j++)
            {
                this.columns[j] += this.columns[j - 1];
            }

            int[] cc = (int[])this.columns.Clone();
            int[] rr = (int[])this.rows.Clone();
            for (int i = 0; i < m; i++)
            {
                for (int j = rows[i]; j < rows[i + 1]; j++)
                {
                    int c = cc[columns[j]];
                    this.columnsRows[c] = i;
                    this.values[c] = values[j];

                    this.rowsColumns[rr[i]] = columns[j];
                    this.positions[rr[i]] = c;

                    cc[columns[j]] += 1;
                    rr[i] += 1;
                }
            }
        }

        public int Count { get; private set; }

        public int Rows { get; private set; }

        public int Columns { get; private set; }

        public int Capacity { get { return values.Length; } }

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

            for (int i = rows[row]; i < rows[row + 1]; i++)
            {
                if (column == rowsColumns[i])
                {
                    return values[positions[i]];
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

            for (int i = rows[row]; i < rows[row + 1]; i++)
            {
                if (column == rowsColumns[i])
                {
                    values[positions[i]] = value;
                    return;
                }
            }

            throw new ArgumentOutOfRangeException();
        }

        public void Sorting()
        {
            for (int j = 0; j < Columns; j++)
            {
                Array.Sort(columnsRows, values, columns[j], columns[j + 1] - columns[j]);
            }

            for (int i = 0; i < Rows; i++)
            {
                Array.Sort(rowsColumns, positions, rows[i], rows[i + 1] - rows[i]);
            }
        }

        public bool Contains(int row, int column)
        {
            if (column < 0 || column >= Columns)
            {
                throw new ArgumentOutOfRangeException($"'column' should be in [0;{Columns})");
            }
            if (row < 0 || row >= Rows)
            {
                throw new ArgumentOutOfRangeException($"'row' should be in [0;{Rows})");
            }

            for (int j = columns[column]; j < columns[column + 1]; j++)
            {
                if (row == columnsRows[j])
                {
                    return true;
                }
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SparseMatrix<T>))
            {
                return false;
            }

            var that = (SparseMatrix<T>)obj;
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

            if (!columns.Take(Columns + 1).SequenceEqual(that.columns.Take(Columns + 1)))
            {
                return false;
            }

            if (!columnsRows.Take(Count).SequenceEqual(that.columnsRows.Take(Count)))
            {
                return false;
            }

            if (!positions.Take(Count).SequenceEqual(that.positions.Take(Count)))
            {
                return false;
            }

            if (!values.Take(Count).SequenceEqual(that.values.Take(Count)))
            {
                return false;
            }

            return true;
        }

        public SparseMatrix<T> Transpose()
        {
            var Y = new SparseMatrix<T>(Columns, Rows, Count);

            Array.Copy(rows, Y.columns, rows.Length);
            Array.Copy(rowsColumns, Y.columnsRows, rowsColumns.Length);
            Array.Copy(columns, Y.rows, columns.Length);
            Array.Copy(columnsRows, Y.rowsColumns, columnsRows.Length);

            for (int j = 0, k = 0; j < Columns; j++)
            {
                for (int i = columns[j]; i < columns[j + 1]; i++)
                {
                    Y.positions[positions[i]] = k;
                    Y.values[k++] = values[positions[i]];
                }
            }

            Y.Count = Count;
            return Y;
        }

        public SparseArray<T> GetRow(int row)
        {
            var array = new SparseArray<T>(Columns);

            for (int j = rows[row]; j < rows[row + 1]; j++)
            {
                array[rowsColumns[j]] = values[positions[j]];
            }

            return array;
        }

        public SparseArray<T> GetColumn(int column)
        {
            var array = new SparseArray<T>(Rows);

            for (int i = columns[column]; i < columns[column + 1]; i++)
            {
                array[columnsRows[i]] = values[i];
            }

            return array;
        }

        public void ForRow(int i, Action<int, T> func)
        {
            for (int j = rows[i]; j < rows[i + 1]; j++)
            {
                func(rowsColumns[j], values[positions[j]]);
            }
        }

        public void ForColumn(int j, Action<int, T> func)
        {
            for (int i = columns[j]; i < columns[j + 1]; i++)
            {
                func(columnsRows[i], values[i]);
            }
        }

        public DenseMatrix<T> ToDenseMatrix()
        {
            var dense = new DenseMatrix<T>(Rows, Columns);
            for (int j = 0; j < Columns; j++)
            {
                for (int i = columns[j]; i < columns[j + 1]; i++)
                {
                    dense[columnsRows[i], j] = values[i];
                }
            }

            return dense;
        }
    }
}
