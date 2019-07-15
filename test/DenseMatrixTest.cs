using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using basp.sharpcode;

namespace basp.sharpcode.test
{
    [TestClass]
    public class DenseMatrixTest
    {
        [TestMethod]
        public void Set()
        {
            var A = new DenseMatrix<double>(3, 3);
            A[0, 0] = 0; A[0, 1] = 1; A[0, 2] = 1;
            A[1, 0] = 2; A[1, 1] = 4; A[1, 2] = -2;
            A[2, 0] = 0; A[2, 1] = 3; A[2, 2] = 15;

            Assert.AreEqual(3, A.Rows);
            Assert.AreEqual(3, A.Columns);

            Assert.AreEqual(0, A[0, 0]);
            Assert.AreEqual(1, A[0, 1]);
            Assert.AreEqual(1, A[0, 2]);

            Assert.AreEqual(2, A[1, 0]);
            Assert.AreEqual(4, A[1, 1]);
            Assert.AreEqual(-2, A[1, 2]);

            Assert.AreEqual(0, A[2, 0]);
            Assert.AreEqual(3, A[2, 1]);
            Assert.AreEqual(15, A[2, 2]);
        }

        [TestMethod]
        public void Set_2()
        {
            var A = new DenseMatrix<double>(3, 2);
            A[0, 0] = 0; A[0, 1] = 1;
            A[1, 0] = 2; A[1, 1] = 4;
            A[2, 0] = 0; A[2, 1] = 3;

            Assert.AreEqual(3, A.Rows);
            Assert.AreEqual(2, A.Columns);

            Assert.AreEqual(0, A[0, 0]);
            Assert.AreEqual(1, A[0, 1]);

            Assert.AreEqual(2, A[1, 0]);
            Assert.AreEqual(4, A[1, 1]);

            Assert.AreEqual(0, A[2, 0]);
            Assert.AreEqual(3, A[2, 1]);
        }

        [TestMethod]
        public void Transpose()
        {
            var A = new DenseMatrix<double>(3, 2);
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
    }
}
