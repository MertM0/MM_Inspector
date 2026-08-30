using System;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMPickerPopup
    {
        private const char SubmenuSafeSlash = '∕';

        private static string[] _withMissing = Array.Empty<string>();

        public static int Draw(Rect position, GUIContent label, string[] options, int selected, string missing)
        {
            string[] display = Escape(options);

            Rect field = EditorGUI.PrefixLabel(position, label);

            if (selected >= 0)
            {
                return EditorGUI.Popup(field, selected, display);
            }

            if (_withMissing.Length != display.Length + 1)
            {
                _withMissing = new string[display.Length + 1];
            }

            _withMissing[0] = missing;

            for (int i = 0; i < display.Length; i++)
            {
                _withMissing[i + 1] = display[i];
            }

            return EditorGUI.Popup(field, 0, _withMissing) - 1;
        }

        public static string Missing(string value)
        {
            return string.IsNullOrEmpty(value) ? "None" : "Missing: " + Escape(value);
        }

        private static string[] Escape(string[] options)
        {
            string[] escaped = null;

            for (int i = 0; i < options.Length; i++)
            {
                string option = Escape(options[i]);

                if (escaped == null && ReferenceEquals(option, options[i]))
                {
                    continue;
                }

                if (escaped == null)
                {
                    escaped = new string[options.Length];

                    for (int copied = 0; copied < i; copied++)
                    {
                        escaped[copied] = options[copied];
                    }
                }

                escaped[i] = option;
            }

            return escaped ?? options;
        }

        private static string Escape(string option)
        {
            return string.IsNullOrEmpty(option) || option.IndexOf('/') < 0
                ? option
                : option.Replace('/', SubmenuSafeSlash);
        }
    }
}
