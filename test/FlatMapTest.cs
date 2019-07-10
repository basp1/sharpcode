using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using sharpcode;

namespace sharpcode.test
{
    [TestClass]
    public class FlatMapTest
    {
        [TestMethod]
        public void Insert_1()
        {
            var map = new FlatMap<int, char>((a, b) => a - b);

            map[1] = 'a';
            map[5] = 'e';
            map[3] = 'c';

            Assert.AreEqual(3, map.Count);
            Assert.AreEqual('a', map[1]);
            Assert.AreEqual('c', map[3]);
            Assert.AreEqual('e', map[5]);

            map[2] = 'b';
            map[4] = 'd';
            Assert.AreEqual(5, map.Count);
            Assert.AreEqual('a', map[1]);
            Assert.AreEqual('b', map[2]);
            Assert.AreEqual('c', map[3]);
            Assert.AreEqual('d', map[4]);
            Assert.AreEqual('e', map[5]);

            map[4] = 'D';
            Assert.AreEqual(5, map.Count);
            Assert.AreEqual('a', map[1]);
            Assert.AreEqual('b', map[2]);
            Assert.AreEqual('c', map[3]);
            Assert.AreEqual('D', map[4]);
            Assert.AreEqual('e', map[5]);
        }

        [TestMethod]
        public void Remove_1()
        {
            var map = new FlatMap<int, char>((a, b) => a - b);

            map[1] = 'a';
            map[5] = 'e';
            map[3] = 'c';
            map[2] = 'b';
            map[4] = 'd';

            map.Remove(3);
            Assert.AreEqual(4, map.Count);
            Assert.AreEqual('a', map[1]);
            Assert.AreEqual('b', map[2]);
            Assert.AreEqual('d', map[4]);
            Assert.AreEqual('e', map[5]);

            map.Remove(1);
            map.Remove(4);
            map.Remove(5);
            Assert.AreEqual(1, map.Count);
            Assert.AreEqual('b', map[2]);
        }
    }
}