using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMIntSetting : MMSetting<int>
    {
        public MMIntSetting(string key, string label, int fallback, int min, int max)
            : base(key, label, Mathf.Clamp(fallback, min, max))
        {
            Min = min;
            Max = max;
        }

        public int Min { get; }

        public int Max { get; }

        protected override int Read(int fallback)
        {
            return EditorPrefs.GetInt(Key, fallback);
        }

        protected override void Write(int value)
        {
            EditorPrefs.SetInt(Key, value);
        }

        protected override int Coerce(int value)
        {
            return Mathf.Clamp(value, Min, Max);
        }
    }
}
