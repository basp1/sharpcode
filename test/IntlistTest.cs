using Microsoft.VisualStudio.TestTools.UnitTesting;
using sharpcode;

namespace sharpcode.test
{
    [TestClass]
    public class IntListTest
    {
        [TestMethod]
        public void Push_1()
        {
            var ii = new IntList(10);
            ii.Push(1);
            ii.Push(7);
            ii.Push(3);
            ii.Push(2);

            Assert.AreEqual(10, ii.Capacity);
            Assert.AreEqual(4, ii.Count);
        }

        [TestMethod]
        public void Contains_1()
        {
            var ii = new IntList(10);
            ii.Push(1);
            ii.Push(7);
            ii.Push(3);
            ii.Push(2);

            Assert.IsTrue(ii.Contains(1));
            Assert.IsTrue(ii.Contains(2));
            Assert.IsTrue(ii.Contains(3));
            Assert.IsTrue(ii.Contains(7));

            Assert.IsFalse(ii.Contains(0));
            Assert.IsFalse(ii.Contains(4));
        }

        [TestMethod]
        public void Pop_1()
        {
            var ii = new IntList(10);
            ii.Push(1);
            ii.Push(7);
            ii.Push(3);
            ii.Push(2);

            Assert.AreEqual(2, ii.Pop());
            Assert.AreEqual(3, ii.Pop());
            Assert.AreEqual(7, ii.Pop());
            Assert.AreEqual(1, ii.Pop());
        }

        [TestMethod]
        public void PopAll_1()
        {
            var ii = new IntList(10);
            ii.Push(1);
            ii.Push(7);
            ii.Push(3);
            ii.Push(2);

            var values = ii.PopAll();

            Assert.AreEqual(0, ii.Count);

            Assert.AreEqual(2, values[0]);
            Assert.AreEqual(3, values[1]);
            Assert.AreEqual(7, values[2]);
            Assert.AreEqual(1, values[3]);
        }
    }
}