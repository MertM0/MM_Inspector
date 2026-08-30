using UnityEditor;

namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMBoolSetting : MMSetting<bool>
    {
        public MMBoolSetting(string key, string label, bool fallback) : base(key, label, fallback)
        {
        }

        protected override bool Read(bool fallback)
        {
            return EditorPrefs.GetBool(Key, fallback);
        }

        protected override void Write(bool value)
        {
            EditorPrefs.SetBool(Key, value);
        }
    }
}
