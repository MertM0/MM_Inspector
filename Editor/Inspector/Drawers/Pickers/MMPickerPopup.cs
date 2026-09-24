using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMPickerPopup
    {
        public const char PathSeparator = '∕';
        public const string None = "None";

        private const string MissingPrefix = "Missing: ";

        public static int Draw(Rect position, GUIContent label, string[] options, int selected, string missing,
            ref string[] withMissing)
        {
            Rect field = EditorGUI.PrefixLabel(position, label);

            if (selected >= 0)
            {
                return EditorGUI.Popup(field, selected, options);
            }

            if (withMissing == null || withMissing.Length != options.Length + 1 || withMissing[0] != missing)
            {
                withMissing = new string[options.Length + 1];
                withMissing[0] = missing;
                options.CopyTo(withMissing, 1);
            }

            return EditorGUI.Popup(field, 0, withMissing) - 1;
        }

        public static string Missing(string value)
        {
            return string.IsNullOrEmpty(value) ? None : MissingPrefix + Escape(value);
        }

        public static string Escape(string option)
        {
            return string.IsNullOrEmpty(option) || option.IndexOf('/') < 0
                ? option
                : option.Replace('/', PathSeparator);
        }
    }
}
