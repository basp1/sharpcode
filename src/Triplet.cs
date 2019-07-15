using System;
using System.Collections.Generic;

namespace basp.sharpcode
{
    public struct Triplet<T>
    {
        public int Column;
        public int Row;
        public T Value;
    }
}