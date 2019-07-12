using System;
using System.Collections.Generic;
using System.Linq;

namespace sharpcode
{
#pragma warning disable CS0659
    public class DenseMatrix<T>
#pragma warning restore CS0659
    {
        int rows;
        int columns;
        T[] values;

        public DenseMatrix(int rows, int columns)
        {
            this.values = new T[rows * columns];
            this.rows = rows;
            this.columns = columns;
        }

        public DenseMatrix(DenseMatrix<T> that)
        {
            rows = that.Rows;
            columns = that.Columns;
            values = new T[Rows * Columns];
            Array.Copy(that.values, values, values.Length);
        }

        public int Rows { get { return rows; } }

        public int Columns { get { return columns; } }

        public T this[int row, int column]
        {
            get
            {
                return values[row * Columns + column];
            }
            set
            {
                values[row * Columns + column] = value;
            }
        }

        public override bool Equals(Object obj)
        {
            if (obj is DenseMatrix<T>)
            {
                return values.SequenceEqual((obj as DenseMatrix<T>).values);
            }
            else
            {
                return false;
            }
        }

        public DenseMatrix<T> Transpose()
        {
            var that = new DenseMatrix<T>(Columns, Rows);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    that[j, i] = this[i, j];
                }
            }

            return that;
        }
    }
}