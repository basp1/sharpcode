using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using sharpcode;

namespace sharpcode.test
{
    [TestClass]
    public class SparseMatrixTest
    {
        [TestMethod]
        public void Constructor()
        {
            var P = new ElasticMatrix<double>(3, 4);
            P[0, 0] = 10; P[0, 1] = 11; P[0, 2] = 12; P[0, 3] = 13;
            P[1, 3] = 23; P[1, 0] = 20; P[1, 1] = 21;
            P[2, 3] = 33; P[2, 2] = 32; P[2, 1] = 31; P[2, 0] = 30;

            var A = new SparseMatrix<double>(P);
            Assert.AreEqual(11, A.Count);

            Assert.AreEqual(10, A[0, 0]);
            Assert.AreEqual(11, A[0, 1]);
            Assert.AreEqual(12, A[0, 2]);
            Assert.AreEqual(13, A[0, 3]);

            Assert.AreEqual(20, A[1, 0]);
            Assert.AreEqual(21, A[1, 1]);
            Assert.AreEqual(23, A[1, 3]);

            Assert.AreEqual(30, A[2, 0]);
            Assert.AreEqual(31, A[2, 1]);
            Assert.AreEqual(32, A[2, 2]);
            Assert.AreEqual(33, A[2, 3]);

            for (int i = 0; i < A.Rows; i++)
            {
                foreach (var it in A.GetRow(i))
                {
                    Assert.AreEqual(A[i, it.Key], it.Value);
                }
            }

            for (int j = 0; j < A.Columns; j++)
            {
                foreach (var it in A.GetColumn(j))
                {
                    Assert.AreEqual(A[it.Key, j], it.Value);
                }
            }
        }

        [TestMethod]
        public void Constructor_2()
        {
            var A = new SparseMatrix<double>(
                3, 4,
                new int[] { 0, 4, 7, 11 },
                new int[] { 0, 1, 2, 3, 0, 1, 3, 0, 1, 2, 3 },
                new double[] { 10, 11, 12, 13, 20, 21, 23, 30, 31, 32, 33 });

            var P = new ElasticMatrix<double>(3, 4);
            P[0, 0] = 10; P[0, 1] = 11; P[0, 2] = 12; P[0, 3] = 13;
            P[1, 3] = 23; P[1, 0] = 20; P[1, 1] = 21;
            P[2, 3] = 33; P[2, 2] = 32; P[2, 1] = 31; P[2, 0] = 30;
            var E = new SparseMatrix<double>(P);

            Assert.IsTrue(A.Equals(E));

            for (int i = 0; i < A.Rows; i++)
            {
                foreach (var it in A.GetRow(i))
                {
                    Assert.AreEqual(E[i, it.Key], it.Value);
                }
            }

            for (int j = 0; j < A.Columns; j++)
            {
                foreach (var it in A.GetColumn(j))
                {
                    Assert.AreEqual(E[it.Key, j], it.Value);
                }
            }
        }

        [TestMethod]
        public void GetRow()
        {
            var P = new ElasticMatrix<double>(3, 4);
            P[0, 0] = 10; P[0, 1] = 11; P[0, 2] = 12; P[0, 3] = 13;
            P[1, 3] = 23; P[1, 0] = 20; P[1, 1] = 21;
            P[2, 3] = 33; P[2, 2] = 32; P[2, 1] = 31; P[2, 0] = 30;

            var A = new SparseMatrix<double>(P);

            var row = A.GetRow(0);
            Assert.AreEqual(4, row.Size);
            Assert.AreEqual(4, row.Count);
            Assert.AreEqual(10, row[0]);
            Assert.AreEqual(11, row[1]);
            Assert.AreEqual(12, row[2]);
            Assert.AreEqual(13, row[3]);

            row = A.GetRow(1);
            Assert.AreEqual(4, row.Size);
            Assert.AreEqual(3, row.Count);
            Assert.AreEqual(20, row[0]);
            Assert.AreEqual(21, row[1]);
            Assert.AreEqual(23, row[3]);

            row = A.GetRow(2);
            Assert.AreEqual(4, row.Size);
            Assert.AreEqual(4, row.Count);
            Assert.AreEqual(30, row[0]);
            Assert.AreEqual(31, row[1]);
            Assert.AreEqual(32, row[2]);
            Assert.AreEqual(33, row[3]);
        }

        [TestMethod]
        public void GetColumn()
        {
            var P = new ElasticMatrix<double>(3, 4);
            P[0, 0] = 10; P[0, 1] = 11; P[0, 2] = 12; P[0, 3] = 13;
            P[1, 3] = 23; P[1, 0] = 20; P[1, 1] = 21;
            P[2, 3] = 33; P[2, 2] = 32; P[2, 1] = 31; P[2, 0] = 30;

            var A = new SparseMatrix<double>(P);

            var column = A.GetColumn(0);
            Assert.AreEqual(3, column.Size);
            Assert.AreEqual(3, column.Count);
            Assert.AreEqual(10, column[0]);
            Assert.AreEqual(20, column[1]);
            Assert.AreEqual(30, column[2]);

            column = A.GetColumn(1);
            Assert.AreEqual(3, column.Size);
            Assert.AreEqual(3, column.Count);
            Assert.AreEqual(11, column[0]);
            Assert.AreEqual(21, column[1]);
            Assert.AreEqual(31, column[2]);

            column = A.GetColumn(2);
            Assert.AreEqual(3, column.Size);
            Assert.AreEqual(2, column.Count);
            Assert.AreEqual(12, column[0]);
            Assert.AreEqual(32, column[2]);

            column = A.GetColumn(3);
            Assert.AreEqual(3, column.Size);
            Assert.AreEqual(3, column.Count);
            Assert.AreEqual(13, column[0]);
            Assert.AreEqual(23, column[1]);
            Assert.AreEqual(33, column[2]);
        }

        [TestMethod]
        public void Transpose()
        {
            var P = new ElasticMatrix<double>(3, 2);
            P[0, 0] = 0; P[0, 1] = 1;
            P[1, 0] = 2; P[1, 1] = 4;
            P[2, 0] = 0; P[2, 1] = 3;

            var A = new SparseMatrix<double>(P);

            var At = A.Transpose();
            var Att = At.Transpose();
            var Attt = Att.Transpose();

            Assert.IsTrue(A.Equals(Att));
            Assert.IsTrue(At.Equals(Attt));
            Assert.IsFalse(A.Equals(At));
        }

        [TestMethod]
        public void ForRow()
        {
            var P = new ElasticMatrix<double>(3, 4);
            P[0, 0] = 10; P[0, 1] = 11; P[0, 2] = 12; P[0, 3] = 13;
            P[1, 3] = 23; P[1, 0] = 20; P[1, 1] = 21;
            P[2, 3] = 33; P[2, 2] = 32; P[2, 1] = 31; P[2, 0] = 30;
            var A = new SparseMatrix<double>(P);

            var row = new List<double>();
            A.ForRow(0, (j, value) => row.Add(value));
            Assert.AreEqual(4, row.Count);
            Assert.AreEqual(10, row[0]);
            Assert.AreEqual(11, row[1]);
            Assert.AreEqual(12, row[2]);
            Assert.AreEqual(13, row[3]);

            row.Clear();
            A.ForRow(1, (j, value) => row.Add(value));
            Assert.AreEqual(3, row.Count);
            Assert.AreEqual(20, row[0]);
            Assert.AreEqual(21, row[1]);
            Assert.AreEqual(23, row[2]);

            row.Clear();
            A.ForRow(2, (j, value) => row.Add(value));
            Assert.AreEqual(4, row.Count);
            Assert.AreEqual(30, row[0]);
            Assert.AreEqual(31, row[1]);
            Assert.AreEqual(32, row[2]);
            Assert.AreEqual(33, row[3]);
        }

        [TestMethod]
        public void ForColumn()
        {
            var P = new ElasticMatrix<double>(3, 4);
            P[0, 0] = 10; P[0, 1] = 11; P[0, 2] = 12; P[0, 3] = 13;
            P[1, 3] = 23; P[1, 0] = 20; P[1, 1] = 21;
            P[2, 3] = 33; P[2, 2] = 32; P[2, 1] = 31; P[2, 0] = 30;
            var A = new SparseMatrix<double>(P);

            var column = new List<double>();
            A.ForColumn(0, (j, value) => column.Add(value));
            Assert.AreEqual(3, column.Count);
            Assert.AreEqual(10, column[0]);
            Assert.AreEqual(20, column[1]);
            Assert.AreEqual(30, column[2]);

            column.Clear();
            A.ForColumn(1, (j, value) => column.Add(value));
            Assert.AreEqual(3, column.Count);
            Assert.AreEqual(11, column[0]);
            Assert.AreEqual(21, column[1]);
            Assert.AreEqual(31, column[2]);

            column.Clear();
            A.ForColumn(2, (j, value) => column.Add(value));
            Assert.AreEqual(2, column.Count);
            Assert.AreEqual(12, column[0]);
            Assert.AreEqual(32, column[1]);

            column.Clear();
            A.ForColumn(3, (j, value) => column.Add(value));
            Assert.AreEqual(3, column.Count);
            Assert.AreEqual(13, column[0]);
            Assert.AreEqual(23, column[1]);
            Assert.AreEqual(33, column[2]);
        }
    }
}