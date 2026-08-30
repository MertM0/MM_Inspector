using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class ResizableTextAreaElement : MMElement
    {
        private readonly MMProperty _property;
        private readonly int _minLines;

        public ResizableTextAreaElement(MMProperty property, int minLines)
        {
            _property = property;
            _minLines = Mathf.Max(1, minLines);
        }

        public override bool IsVisible => _property.IsVisible;

        protected override float CalculateHeight(float width)
        {
            SerializedProperty serialized = _property.Serialized;

            int lines = Mathf.Max(_minLines, CountLines(serialized.stringValue));
            return EditorGUIUtility.singleLineHeight + lines * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position)
        {
            SerializedProperty serialized = _property.Serialized;

            Rect label = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(label, _property.Label);

            Rect area = new Rect(position.x, label.yMax, position.width, position.height - label.height);

            using (new EditorGUI.DisabledScope(!_property.IsEnabled))
            using (new MMMixedValueScope(_property))
            {
                EditorGUI.BeginChangeCheck();

                string edited = EditorGUI.TextArea(area, serialized.stringValue, EditorStyles.textArea);

                if (EditorGUI.EndChangeCheck())
                {
                    serialized.stringValue = edited;
                }
            }
        }

        private static int CountLines(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 1;
            }

            int lines = 1;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    lines++;
                }
            }

            return lines;
        }
    }
}
