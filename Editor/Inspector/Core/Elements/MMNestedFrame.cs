using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMNestedFrame
    {
        public const float Overlap = 1f;

        private const float SidePadding = 6f;
        private const float TopPadding = 3f;
        private const float BottomPadding = 5f;

        private static readonly MMStyleCache Background = new MMStyleCache(BuildBackground);

        public static float Inset => SidePadding;

        public static float VerticalPadding => TopPadding + BottomPadding;

        public static Rect Content(Rect rect)
        {
            return new Rect(
                rect.x + SidePadding,
                rect.y + TopPadding,
                Mathf.Max(0f, rect.width - SidePadding * 2f),
                Mathf.Max(0f, rect.height - VerticalPadding));
        }

        public static void DrawBackground(Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
            {
                Background.Style.Draw(rect, false, false, false, false);
            }
        }

        private static GUIStyle BuildBackground()
        {
            GUIStyle source = GUI.skin.FindStyle("RL Background") ?? EditorStyles.helpBox;

            return new GUIStyle(source)
            {
                fixedHeight = 0f,
                stretchHeight = true,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0)
            };
        }
    }
}
