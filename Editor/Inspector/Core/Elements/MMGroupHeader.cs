using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMGroupHeader
    {
        public const float Height = 23f;

        private const float ContentInset = 6f;
        private const float ArrowInset = 18f;

        private static GUIStyle _background;

        public static void Draw(Rect rect, string label)
        {
            DrawBackground(rect);
            EditorGUI.LabelField(Inset(rect, ContentInset), label);
        }

        public static bool DrawFoldout(Rect rect, bool expanded, string label)
        {
            DrawBackground(rect);
            return EditorGUI.Foldout(Inset(rect, ArrowInset), expanded, label, toggleOnLabelClick: true);
        }

        public static void DrawBackground(Rect rect)
        {
            EnsureStyle();

            if (Event.current.type == EventType.Repaint)
            {
                _background.Draw(rect, false, false, false, false);
            }
        }

        public static Rect Inset(Rect rect, float left)
        {
            float height = EditorGUIUtility.singleLineHeight;

            return new Rect(
                rect.x + left,
                rect.y + (rect.height - height) * 0.5f,
                Mathf.Max(0f, rect.width - left - ContentInset),
                height);
        }

        private static void EnsureStyle()
        {
            if (_background != null)
            {
                return;
            }

            GUIStyle source = GUI.skin.FindStyle("RL Header") ?? EditorStyles.toolbar;

            _background = new GUIStyle(source)
            {
                fixedHeight = 0f,
                stretchHeight = true
            };
        }
    }
}
