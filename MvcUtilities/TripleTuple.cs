using System.Collections.Generic;

namespace MvcUtilities
{
    public struct TripleTuple<T1, T2, T3>
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }
        public T3 Third { get; set; }

        public TripleTuple(T1 x, T2 y, T3 z) : this()
        {
            First = x;
            Second = y;
            Third = z;
        }
    }

    public class TripleTupleList<T1, T2, T3> : List<TripleTuple<T1, T2, T3>>
    {
        public void Add(T1 first, T2 second, T3 third)
        {
            Add(new TripleTuple<T1, T2, T3>(first, second, third));
        }
    }
}
