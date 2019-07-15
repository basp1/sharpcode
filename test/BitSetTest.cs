using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using basp.sharpcode;

namespace basp.sharpcode.test
{
    [TestClass]
    public class BitSetTest
    {
        [TestMethod]
        public void Set_1()
        {
            var b = new BitSet(10);
            Assert.AreEqual(1, b.Values.Length);

            b[1] = true;
            Assert.AreEqual(2, b.Values[0]);

            b[0] = true;
            Assert.AreEqual(3, b.Values[0]);

            b[7] = true;
            Assert.AreEqual(131, b.Values[0]);

            b[0] = false;
            Assert.AreEqual(130, b.Values[0]);

            b[1] = false;
            Assert.AreEqual(128, b.Values[0]);

            b[7] = false;
            Assert.AreEqual(0, b.Values[0]);
        }

        [TestMethod]
        public void Set_2()
        {
            var b = new BitSet(1);
            Assert.AreEqual(1, b.Values.Length);

            b = new BitSet(32);
            Assert.AreEqual(1, b.Values.Length);

            b = new BitSet(33);
            Assert.AreEqual(2, b.Values.Length);

            b = new BitSet(63);
            Assert.AreEqual(2, b.Values.Length);

            b = new BitSet(64);
            Assert.AreEqual(2, b.Values.Length);

            b = new BitSet(65);
            Assert.AreEqual(3, b.Values.Length);
        }
        [TestMethod]
        public void Set_3()
        {
            var b = new BitSet(100);
            Assert.AreEqual(4, b.Values.Length);

            for (int i = 0; i < 8; i++)
                b[i] = true;
            Assert.AreEqual(255, b.Values[0]);

            for (int i = 8; i < 16; i++)
                b[i] = true;
            Assert.AreEqual(65535, b.Values[0]);

            for (int i = 0; i < 8; i++)
                b[i] = false;
            Assert.AreEqual(65280, b.Values[0]);

            for (int i = 8; i < 16; i++)
                b[i] = false;
            Assert.AreEqual(0, b.Values[0]);

            b[16] = true;
            Assert.AreEqual(65536, b.Values[0]);

            b[33] = true;
            Assert.AreEqual(2, b.Values[1]);
        }

        [TestMethod]
        public void Get_1()
        {
            var b = new BitSet(100);
            Assert.AreEqual(4, b.Values.Length);

            b[3] = true;
            Assert.IsFalse(b[0]);
            Assert.IsFalse(b[1]);
            Assert.IsFalse(b[2]);
            Assert.IsTrue(b[3]);
            Assert.IsFalse(b[4]);
            Assert.IsFalse(b[5]);

            b[50] = true;
            Assert.IsFalse(b[0]);
            Assert.IsFalse(b[18]);
            Assert.IsTrue(b[50]);
            Assert.IsFalse(b[72]);
        }
    }
}

