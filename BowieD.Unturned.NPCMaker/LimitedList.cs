using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker
{
    public static class LimitedListLinq
    {
        public static LimitedList<T> ToLimitedList<T>(this IEnumerable<T> ts, int maxItems)
        {
            return new LimitedList<T>(ts, maxItems);
        }
    }
    public class LimitedList<T> : List<T>
    {
        public LimitedList(IEnumerable<T> collection, int maxItems) : base(maxItems)
        {
            this.MaxItems = maxItems;

            foreach (var item in collection)
                Add(item);
        }
        public LimitedList(int maxItems) : base(maxItems)
        {
            this.MaxItems = maxItems;
        }

        public int MaxItems { get; }

        public new void Add(T element)
        {
            if (CanAdd)
                base.Add(element);
        }
        public new void Insert(int index, T element)
        {
            if (CanAdd)
                base.Insert(index, element);
        }

        public bool CanAdd => Count < MaxItems;
    }
}
