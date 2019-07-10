using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using sharpcode;

namespace sharpcode.test
{
    [TestClass]
    public class PriorityQueueTest
    {
        [TestMethod]
        public void Push_1()
        {
            var pq = new PriorityQueue<int>(Math.Min);

            pq.Push(3);
            Assert.AreEqual(3, pq.Top);
            Assert.AreEqual(1, pq.Height);

            pq.Push(4);
            Assert.AreEqual(3, pq.Top);
            Assert.AreEqual(2, pq.Height);

            pq.Push(5);
            Assert.AreEqual(3, pq.Top);
            Assert.AreEqual(2, pq.Height);

            pq.Push(2);
            Assert.AreEqual(2, pq.Top);
            Assert.AreEqual(3, pq.Height);

            pq.Push(1);
            Assert.AreEqual(1, pq.Top);
            Assert.AreEqual(3, pq.Height);
        }

        [TestMethod]
        public void Push_2()
        {
            var pq = new PriorityQueue<int>(Math.Min);

            for (int i = 8; i >= 0; i--)
            {
                pq.Push(i);
            }

            Assert.AreEqual(0, pq.Top);
            Assert.AreEqual(4, pq.Height);

            pq.Push(9);
            Assert.AreEqual(0, pq.Top);
            Assert.AreEqual(4, pq.Height);
        }

        [TestMethod]
        public void Push_3()
        {
            var pq = new PriorityQueue<int>(Math.Min, 5);
            for (int i = 8; i >= 0; i--)
            {
                pq.Push(i);
            }

            Assert.AreEqual(0, pq.Top);
            Assert.AreEqual(4, pq.Height);

            pq.Push(9);
            Assert.AreEqual(0, pq.Top);
            Assert.AreEqual(4, pq.Height);
        }

        [TestMethod]
        public void Pop_1()
        {
            var pq = new PriorityQueue<int>(Math.Min);

            pq.Push(18);
            pq.Push(19);
            pq.Push(20);
            Assert.AreEqual(18, pq.Top);

            pq.Pop();
            Assert.AreEqual(19, pq.Top);

            pq.Pop();
            Assert.AreEqual(20, pq.Top);

            pq.Pop();
            Assert.AreEqual(0, pq.Count);
        }

        [TestMethod]
        public void Pop_2()
        {
            var pq = new PriorityQueue<int>(Math.Min);
            for (int i = 8; i >= 0; i--)
            {
                pq.Push(i);
            }

            Assert.AreEqual(0, pq.Top);
            Assert.AreEqual(4, pq.Height);

            pq.Pop();
            Assert.AreEqual(1, pq.Top);
            Assert.AreEqual(4, pq.Height);

            pq.Pop();
            Assert.AreEqual(2, pq.Top);
            Assert.AreEqual(3, pq.Height);

            pq.Pop();
            Assert.AreEqual(3, pq.Top);
            Assert.AreEqual(3, pq.Height);

            pq.Pop();
            Assert.AreEqual(4, pq.Top);
            Assert.AreEqual(3, pq.Height);
        }

        [TestMethod]
        public void Pop_3()
        {
            const int N = 20;
            var pq = new PriorityQueue<int>(Math.Min);

            for (int i = N; i > 0; i--)
            {
                pq.Push(i);
            }

            Assert.AreEqual(1, pq.Top);

            for (int i = 0; i < (int)(N / 2); i++)
            {
                pq.Pop();
            }
            Assert.AreEqual(1 + N / 2, pq.Top);

            for (int i = 0; i < (int)((N / 2) - 1); i++)
            {
                pq.Pop();
            }

            Assert.AreEqual(N, pq.Top);
        }
    }
}
