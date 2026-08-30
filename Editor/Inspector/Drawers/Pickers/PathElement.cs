using System.IO;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal abstract class PathElement : MMElement
    {
        private const float ButtonWidth = 30f;
        private const float Gap = 2f;

        private static readonly GUIContent PickerLabel = new GUIContent("...", "Browse");

        private readonly MMProperty _property;
        private readonly bool _absolute;

        protected PathElement(MMProperty property, bool absolute)
        {
            _property = property;
            _absolute = absolute;
        }

        public override bool IsVisible => _property.IsVisible;

        protected abstract string OpenPanel(string startPath);

        protected override float CalculateHeight(float width)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position)
        {
            SerializedProperty serialized = _property.Serialized;

            Rect field = EditorGUI.PrefixLabel(position, _property.Label);
            Rect text = new Rect(field.x, field.y, field.width - ButtonWidth - Gap, field.height);
            Rect button = new Rect(text.xMax + Gap, field.y, ButtonWidth, field.height);

            using (new EditorGUI.DisabledScope(!_property.IsEnabled))
            using (new MMMixedValueScope(_property))
            {
                EditorGUI.BeginChangeCheck();

                string edited = EditorGUI.TextField(text, serialized.stringValue);

                if (EditorGUI.EndChangeCheck())
                {
                    serialized.stringValue = edited;
                }

                if (!GUI.Button(button, PickerLabel, EditorStyles.miniButton))
                {
                    return;
                }

                string picked = OpenPanel(MMPathUtility.ToAbsolute(serialized.stringValue));

                if (string.IsNullOrEmpty(picked))
                {
                    return;
                }

                serialized.stringValue = _absolute
                    ? MMPathUtility.Normalize(picked)
                    : MMPathUtility.ToProjectRelative(picked);

                GUI.changed = true;
            }
        }

        protected static string ResolveDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return Application.dataPath;
            }

            if (Directory.Exists(path))
            {
                return path;
            }

            string parent = Path.GetDirectoryName(path);

            return string.IsNullOrEmpty(parent) || !Directory.Exists(parent) ? Application.dataPath : parent;
        }
    }
}
