using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class TitleElement : DecoratedElement
    {
        private const float TopSpace = 6f;
        private const float LineSpace = 2f;
        private const float LineHeight = 1f;

        private readonly string _text;
        private readonly bool _line;

        private static GUIStyle _style;

        public TitleElement(MMProperty property, MMElement inner, string text, bool line)
            : base(property, inner)
        {
            _text = text;
            _line = line;
        }

        protected override float GetDecorationHeight(float width)
        {
            float height = TopSpace;

            if (!string.IsNullOrEmpty(_text))
            {
                height += EditorGUIUtility.singleLineHeight;
            }

            if (_line)
            {
                height += LineSpace + LineHeight + LineSpace;
            }

            return height;
        }

        protected override void DrawDecoration(Rect rect)
        {
            EnsureStyle();

            float y = rect.y + TopSpace;

            if (!string.IsNullOrEmpty(_text))
            {
                EditorGUI.LabelField(new Rect(rect.x, y, rect.width, EditorGUIUtility.singleLineHeight), _text, _style);
                y += EditorGUIUtility.singleLineHeight;
            }

            if (!_line)
            {
                return;
            }

            y += LineSpace;
            EditorGUI.DrawRect(new Rect(rect.x, y, rect.width, LineHeight), LineColor);
        }

        private static Color LineColor => EditorGUIUtility.isProSkin
            ? new Color(0.35f, 0.35f, 0.35f)
            : new Color(0.6f, 0.6f, 0.6f);

        private static void EnsureStyle()
        {
            if (_style == null)
            {
                _style = new GUIStyle(EditorStyles.label) { fontStyle = FontStyle.Bold };
            }
        }
    }
}
