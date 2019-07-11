using System;
using System.Collections.Generic;

namespace sharpcode
{
    public struct Triplet<T>
    {
        public int column;
        public int row;
        public T value;
    }
}