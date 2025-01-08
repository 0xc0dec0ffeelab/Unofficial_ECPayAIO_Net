using System;
using System.Collections;
using System.Collections.Generic;

namespace ECPay.Payment.Integration
{
    public class ItemCollection : IList<Item>
    {
        public event ItemCollectionEventHandler? CollectionChanged;

        private readonly List<Item> _items = [];

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public Item this[int index]
        {
            get => _items[index];
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                _items[index] = value;
            }
        }

        public void Add(Item item)
        {
            ArgumentNullException.ThrowIfNull(item);
            _items.Add(item);
            RaiseCollectionEvents("Add");
        }

        public void AddRange(IEnumerable<Item> collection)
        {
            _items.AddRange(collection);
            RaiseCollectionEvents("AddRange");
        }

        public void Clear()
        {
            _items.Clear();
            RaiseCollectionEvents("Clear");
        }

        public void Insert(int index, Item item)
        {
            ArgumentNullException.ThrowIfNull(item);
            _items.Insert(index, item);
            RaiseCollectionEvents("Insert");
        }

        public void InsertRange(int index, IEnumerable<Item> collection)
        {
            _items.InsertRange(index, collection);
            RaiseCollectionEvents("InsertRange");
        }

        public bool Remove(Item item)
        {
            bool result = _items.Remove(item);
            RaiseCollectionEvents("Remove");
            return result;
        }

        public int RemoveAll(Predicate<Item> match)
        {
            int result = _items.RemoveAll(match);
            RaiseCollectionEvents("RemoveAll");
            return result;
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
            RaiseCollectionEvents("RemoveAt");
        }

        public void RemoveRange(int index, int count)
        {
            _items.RemoveRange(index, count);
            RaiseCollectionEvents("RemoveRange");
        }

        protected virtual void RaiseCollectionEvents(string methodName)
        {
            CollectionChanged?.Invoke(this, new ItemCollectionEventArgs(this, methodName));
        }

        public int IndexOf(Item item) => _items.IndexOf(item);

        public bool Contains(Item item) => _items.Contains(item);

        public void CopyTo(Item[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<Item> GetEnumerator() => _items.GetEnumerator();
    }
}
