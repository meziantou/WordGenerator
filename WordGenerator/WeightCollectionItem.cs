using System.Diagnostics;

namespace WordGenerator
{
    [DebuggerDisplay("{Item}: {Weight}")]
    internal class WeightCollectionItem<T>
    {
        public WeightCollectionItem(T item)
        {
            Item = item;
        }

        public T Item { get; }
        public int Weight { get; set; } = 1;
    }
}