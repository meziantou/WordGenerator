using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WordGenerator
{
    internal class WeightCollection<T> : IEnumerable<WeightCollectionItem<T>>
    {
        private readonly List<WeightCollectionItem<T>> _list = new List<WeightCollectionItem<T>>();

        public int TotalWeight { get; private set; }

        public void Add(T item)
        {
            TotalWeight++;

            var weightItem = Find(item);
            if (weightItem != null)
            {
                weightItem.Weight += 1;
            }
            else
            {
                _list.Add(new WeightCollectionItem<T>(item));
            }
        }

        private WeightCollectionItem<T> Find(T item)
        {
            return _list.FirstOrDefault(_ => Equals(_.Item, item));
        }

        public void Sort()
        {
            _list.Sort((a, b) => a.Weight.CompareTo(b.Weight));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<WeightCollectionItem<T>>)this).GetEnumerator();
        }

        IEnumerator<WeightCollectionItem<T>> IEnumerable<WeightCollectionItem<T>>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}