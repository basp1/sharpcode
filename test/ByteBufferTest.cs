using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using basp.sharpcode;

namespace basp.sharpcode.test
{
    [TestClass]
    public class ByteBufferTest
    {

        [TestMethod]
        public void Test_1()
        {
            var bb = new ByteBuffer(6);
            Assert.AreEqual(0, bb.Position);

            bb.Put(1);
            Assert.AreEqual(1, bb.Position);

            bb.PutInt(2);
            Assert.AreEqual(5, bb.Position);

            bb.Flip();
            Assert.AreEqual(1, (int)bb.Get());
            Assert.AreEqual(2, (int)bb.GetInt());

            bb.Flip();
            bb.PutInt(3);
            bb.PutLong(4);
            Assert.IsTrue(bb.ToArray().Length >= 12);
            Assert.AreEqual(12, bb.Position);
            Assert.AreEqual(3, (int)bb.GetInt(0));
            Assert.AreEqual(4, (int)bb.GetLong(4));

            bb.Position = 4;
            Assert.AreEqual(4, (int)bb.GetLong());

            bb.Flip();
            Assert.AreEqual(3, (int)bb.GetInt());
        }
    }
}