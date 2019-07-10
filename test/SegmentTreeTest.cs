using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using sharpcode;

namespace sharpcode.test
{
    [TestClass]
    public class SegmentTreeTest
    {
        [TestMethod]
        public void Range_1()
        {
            var u = new SegmentTree<int>(Int32.MinValue, Math.Max, new int[] { 1 });

            Assert.AreEqual(1, u.Top);
            Assert.AreEqual(1, u.Range(0, 0));
            Assert.AreEqual(1, u[0]);
        }

        [TestMethod]
        public void Range_2()
        {
            var u = new SegmentTree<int>(Int32.MinValue, Math.Max, new int[] { 3, 8, 6, 4, 2, 5, 9, 0, 7, 1 });

            Assert.AreEqual(9, u.Top);
            Assert.AreEqual(9, u.Range(0, 9));
            Assert.AreEqual(9, u.Range(5, 9));
            Assert.AreEqual(8, u.Range(0, 4));
            Assert.AreEqual(8, u.Range(1, 1));
            Assert.AreEqual(8, u.Range(1, 4));
            Assert.AreEqual(7, u.Range(7, 8));
            Assert.AreEqual(5, u.Range(4, 5));
            Assert.AreEqual(6, u.Range(2, 5));
        }

        [TestMethod]
        public void Range_3()
        {
            var u = new SegmentTree<int>(Int32.MinValue, Math.Max, new int[] { 5, 4, 3, 2, 1 });

            Assert.AreEqual(5, u.Top);
            Assert.AreEqual(3, u.Range(2, 4));
            Assert.AreEqual(5, u[0]);
            Assert.AreEqual(4, u[1]);
            Assert.AreEqual(3, u[2]);
        }

        [TestMethod]
        public void Set_1()
        {
            var values = new int[] { 3, 8, 6, 4, 2, 5, 9, 0, 7, 1 };
            var u = new SegmentTree<int>(Int32.MinValue, Math.Max, values);

            var v = new SegmentTree<int>(Int32.MinValue, Math.Max, dim: 10);

            Assert.IsFalse(u.Equals(v));

            for (int i = 0; i < 10; i++)
            {
                v[i] = values[i];
            }

            Assert.IsTrue(u.Equals(v));
        }

        [TestMethod]
        public void Set_2()
        {
            var u = new SegmentTree<int>(Int32.MinValue, Math.Max, new int[] { 3, 8, 6, 4, 2, 5, 9, 0, 7, 1 });

            u[6] = 0;

            Assert.AreEqual(8, u.Top);
            Assert.AreEqual(8, u.Range(0, 9));
            Assert.AreEqual(7, u.Range(5, 9));
            Assert.AreEqual(8, u.Range(0, 4));
            Assert.AreEqual(8, u[1]);
            Assert.AreEqual(8, u.Range(1, 4));
            Assert.AreEqual(7, u.Range(7, 8));
            Assert.AreEqual(5, u.Range(4, 5));
            Assert.AreEqual(6, u.Range(2, 5));
        }
    }
}