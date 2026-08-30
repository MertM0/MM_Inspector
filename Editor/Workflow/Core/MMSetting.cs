using System.Collections.Generic;
using UnityEditor;

namespace MM.Inspector.Workflow.Editor
{
    public abstract class MMSetting<T>
    {
        private readonly T _fallback;

        private bool _loaded;
        private T _value;

        protected MMSetting(string key, string label, T fallback)
        {
            Key = key;
            Label = label;
            _fallback = fallback;
        }

        public string Key { get; }

        public string Label { get; }

        public T Value
        {
            get
            {
                if (!_loaded)
                {
                    _loaded = true;
                    _value = Coerce(Read(_fallback));
                }

                return _value;
            }
            set
            {
                T coerced = Coerce(value);

                if (_loaded && EqualityComparer<T>.Default.Equals(_value, coerced))
                {
                    return;
                }

                _loaded = true;
                _value = coerced;
                Write(coerced);
            }
        }

        public void Reload()
        {
            _loaded = false;
        }

        public void Reset()
        {
            EditorPrefs.DeleteKey(Key);
            _loaded = false;
        }

        protected abstract T Read(T fallback);

        protected abstract void Write(T value);

        protected virtual T Coerce(T value)
        {
            return value;
        }
    }
}
