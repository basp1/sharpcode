using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using sharpcode;

namespace sharpcode.test
{
    [TestClass]
    public class ElasticMatrixTest
    {
        [TestMethod]
        public void Transpose()
        {
            var A = new ElasticMatrix<int>(3, 2);
            A[0, 0] = 0; A[0, 1] = 1;
            A[1, 0] = 2; A[1, 1] = 4;
            A[2, 0] = 0; A[2, 1] = 3;

            var At = A.Transpose();
            var Att = At.Transpose();
            var Attt = Att.Transpose();

            Assert.IsTrue(A.Equals(Att));
            Assert.IsTrue(At.Equals(Attt));
            Assert.IsFalse(A.Equals(At));
        }

        [TestMethod]
        public void Sorting()
        {
            var A = new ElasticMatrix<int>(3, 4);
            A[0, 0] = 10; A[0, 1] = 11; A[0, 2] = 12; A[0, 3] = 13;
            A[1, 3] = 23; A[1, 0] = 20; A[1, 1] = 21;
            A[2, 3] = 33; A[2, 2] = 32; A[2, 1] = 31; A[2, 0] = 30;

            var B = new ElasticMatrix<int>(A);

            Assert.IsTrue(A.Equals(B));

            A.Sorting();

            Assert.IsFalse(A.Equals(B));

            var E = new ElasticMatrix<int>(3, 4);
            E[0, 0] = 10; E[0, 1] = 11; E[0, 2] = 12; E[0, 3] = 13;
            E[1, 0] = 20; E[1, 1] = 21; E[1, 3] = 23;
            E[2, 0] = 30; E[2, 1] = 31; E[2, 2] = 32; E[2, 3] = 33;

            Assert.IsTrue(E.Equals(A));
        }

        [TestMethod]
        public void GetRow()
        {
            var A = new ElasticMatrix<int>(3, 4);
            A[0, 0] = 10; A[0, 1] = 11; A[0, 2] = 12; A[0, 3] = 13;
            A[1, 3] = 23; A[1, 0] = 20; A[1, 1] = 21;
            A[2, 3] = 33; A[2, 2] = 32; A[2, 1] = 31; A[2, 0] = 30;

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
        public void ForRow()
        {
            var A = new ElasticMatrix<double>(3, 4);
            A[0, 0] = 10; A[0, 1] = 11; A[0, 2] = 12; A[0, 3] = 13;
            A[1, 3] = 23; A[1, 0] = 20; A[1, 1] = 21;
            A[2, 3] = 33; A[2, 2] = 32; A[2, 1] = 31; A[2, 0] = 30;

            A.Sorting();

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
    }
}