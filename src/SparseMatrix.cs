﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace sharpcode
{
    public class SparseMatrix<T>
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
    }
}