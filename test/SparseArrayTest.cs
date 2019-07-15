using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using basp.sharpcode;

namespace basp.sharpcode.test
{
    [TestClass]
    public class SparseArrayTest
    {
        [TestMethod]
        public void Set_1()
        {
            var a = new SparseArray<char>(10);

            a.Set(1, 'a');
            a.Set(5, 'e');
            a.Set(3, 'c');

            Assert.AreEqual(3, a.Count);

            a.Set(2, 'b');
            a.Set(4, 'd');

            Assert.AreEqual(5, a.Count);
        }

        [TestMethod]
        public void Set_2()
        {
            var a = new SparseArray<char>(10);

            a.Set(1, 'a');
            Assert.AreEqual(1, a.Count);

            a.Set(1, 'a');
            Assert.AreEqual(1, a.Count);

            a.Set(1, 'b');
            Assert.AreEqual(1, a.Count);
        }

        [TestMethod]
        public void Contains_1()
        {
            var a = new SparseArray<char>(10);

            a.Set(1, 'a');
            a.Set(5, 'e');
            a.Set(3, 'c');

            Assert.IsTrue(a.Contains(1));
            Assert.IsTrue(a.Contains(3));
            Assert.IsTrue(a.Contains(5));
            Assert.IsFalse(a.Contains(0));
            Assert.IsFalse(a.Contains(2));
            Assert.IsFalse(a.Contains(4));
            Assert.IsFalse(a.Contains(6));

            a.Set(2, 'b');
            a.Set(4, 'd');

            Assert.IsTrue(a.Contains(1));
            Assert.IsTrue(a.Contains(2));
            Assert.IsTrue(a.Contains(3));
            Assert.IsTrue(a.Contains(4));
            Assert.IsTrue(a.Contains(5));
            Assert.IsFalse(a.Contains(0));
            Assert.IsFalse(a.Contains(6));
        }

        [TestMethod]
        public void Contains_2()
        {
            var a = new SparseArray<char>(10);

            a.Set(1, 'a');
            Assert.IsTrue(a.Contains(1));
            Assert.IsFalse(a.Contains(0));
            Assert.IsFalse(a.Contains(9));

            a.Set(1, 'a');
            Assert.IsTrue(a.Contains(1));
            Assert.IsFalse(a.Contains(0));
            Assert.IsFalse(a.Contains(9));

            a.Set(1, 'b');
            Assert.IsTrue(a.Contains(1));
            Assert.IsFalse(a.Contains(0));
            Assert.IsFalse(a.Contains(9));
        }

        [TestMethod]
        public void Get_1()
        {
            var a = new SparseArray<char>(10);

            a.Set(1, 'a');
            a.Set(5, 'e');
            a.Set(3, 'c');

            Assert.AreEqual('a', a.Get(1));
            Assert.AreEqual('c', a.Get(3));
            Assert.AreEqual('e', a.Get(5));

            a.Set(2, 'b');
            a.Set(4, 'd');

            Assert.AreEqual('a', a.Get(1));
            Assert.AreEqual('b', a.Get(2));
            Assert.AreEqual('c', a.Get(3));
            Assert.AreEqual('d', a.Get(4));
            Assert.AreEqual('e', a.Get(5));
        }

        [TestMethod]
        public void Get_2()
        {
            var a = new SparseArray<char>(10);

            a.Set(1, 'a');
            Assert.AreEqual('a', a.Get(1));

            a.Set(1, 'a');
            Assert.AreEqual('a', a.Get(1));

            a.Set(1, 'b');
            Assert.AreEqual('b', a.Get(1));
        }

        [TestMethod]
        public void Equals_1()
        {
            var a = new SparseArray<char>(10, new KeyValuePair<int, char>[]
            {
                new KeyValuePair<int, char>(1, 'a'),
                new KeyValuePair<int, char>(2, 'b'),
                new KeyValuePair<int, char>(3, 'c'),
                new KeyValuePair<int, char>(4, 'd'),
                new KeyValuePair<int, char>(5, 'e')
            });

            var b = new SparseArray<char>(10);
            b[1] = 'a';
            b[5] = 'e';
            b[3] = 'c';
            b[2] = 'b';
            b[4] = 'd';

            Assert.IsTrue(a.Equals(b));
        }

        [TestMethod]
        public void Equals_2()
        {
            var a = new SparseArray<char>(10);
            a.Set(1, 'a');
            a.Set(2, 'b');
            a.Set(3, 'c');
            a.Set(4, 'd');
            a.Set(5, 'e');

            var b = new SparseArray<char>(10);
            b.Set(1, 'a');
            b.Set(5, 'e');
            b.Set(3, 'c');
            b.Set(2, 'b');
            b.Set(4, 'd');

            Assert.IsTrue(b.Equals(a));
        }

        [TestMethod]
        public void Equals_3()
        {
            var a = new SparseArray<char>(20);
            a.Set(1, 'a');
            a.Set(2, 'b');
            a.Set(3, 'c');
            a.Set(4, 'd');
            a.Set(5, 'e');

            var b = new SparseArray<char>(10);
            b.Set(1, 'a');
            b.Set(5, 'e');
            b.Set(3, 'c');
            b.Set(2, 'b');
            b.Set(4, 'd');

            Assert.IsFalse(b.Equals(a));
        }

        [TestMethod]
        public void Equals_4()
        {
            var a = new SparseArray<char>(20);
            a[1] = 'a';

            var b = new SparseArray<char>(10);
            b[1] = 'b';

            Assert.IsFalse(b.Equals(a));
        }

        [TestMethod]
        public void Equals_5()
        {
            var a = new SparseArray<char>(4, new KeyValuePair<int, char>[]
            {
                new KeyValuePair<int, char>(0, 'a'),
                new KeyValuePair<int, char>(1, 'b'),
                new KeyValuePair<int, char>(2, 'c'),
                new KeyValuePair<int, char>(3, 'd'),
            });

            var b = new SparseArray<char>(new char[] { 'a', 'b', 'c', 'd' });

            Assert.IsTrue(a.Equals(b));
        }
    }
}