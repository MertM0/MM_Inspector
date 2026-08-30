using System.Collections;
using System.Collections.Generic;

namespace MM.Inspector
{
    public sealed class DropdownList<T> : IEnumerable<KeyValuePair<string, T>>
    {
        private readonly List<KeyValuePair<string, T>> _items = new List<KeyValuePair<string, T>>();

        public int Count => _items.Count;

        public void Add(string label, T value)
        {
            _items.Add(new KeyValuePair<string, T>(label, value));
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
