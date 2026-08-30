using System.Collections.Generic;

namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMHistoryStack
    {
        private readonly List<int> _entries = new List<int>();
        private readonly int _capacity;
        private int _index = -1;

        public MMHistoryStack(int capacity)
        {
            _capacity = capacity < 1 ? 1 : capacity;
        }

        public int Count => _entries.Count;

        public bool CanGoBack => _index > 0;

        public bool CanGoForward => _index >= 0 && _index < _entries.Count - 1;

        public int Current => _index < 0 ? 0 : _entries[_index];

        public void Push(int id)
        {
            if (id == 0)
            {
                return;
            }

            if (_index >= 0 && _entries[_index] == id)
            {
                return;
            }

            if (_index < _entries.Count - 1)
            {
                _entries.RemoveRange(_index + 1, _entries.Count - _index - 1);
            }

            _entries.Add(id);

            if (_entries.Count > _capacity)
            {
                _entries.RemoveAt(0);
            }

            _index = _entries.Count - 1;
        }

        public int GoBack()
        {
            if (!CanGoBack)
            {
                return 0;
            }

            _index--;
            return _entries[_index];
        }

        public int GoForward()
        {
            if (!CanGoForward)
            {
                return 0;
            }

            _index++;
            return _entries[_index];
        }

        public string Serialize()
        {
            string[] parts = new string[_entries.Count + 1];
            parts[0] = _index.ToString();

            for (int i = 0; i < _entries.Count; i++)
            {
                parts[i + 1] = _entries[i].ToString();
            }

            return string.Join(",", parts);
        }

        public static MMHistoryStack Deserialize(string text, int capacity)
        {
            MMHistoryStack stack = new MMHistoryStack(capacity);

            if (string.IsNullOrEmpty(text))
            {
                return stack;
            }

            string[] parts = text.Split(',');
            int index;

            if (!int.TryParse(parts[0], out index))
            {
                return stack;
            }

            for (int i = 1; i < parts.Length; i++)
            {
                int id;

                if (int.TryParse(parts[i], out id))
                {
                    stack._entries.Add(id);
                }
            }

            stack._index = index < stack._entries.Count ? index : stack._entries.Count - 1;
            return stack;
        }
    }
}
