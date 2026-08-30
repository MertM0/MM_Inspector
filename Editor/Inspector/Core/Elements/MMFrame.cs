using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMFrame
    {
        public static readonly RectOffset Padding = new RectOffset(4, 4, 4, 4);

        public static readonly RectOffset BodyPadding = new RectOffset(4, 4, 2, 4);

        public static readonly RectOffset NoPadding = new RectOffset(0, 0, 0, 0);

        public static void Draw(Rect rect)
        {
            GUI.Box(rect, GUIContent.none, EditorStyles.helpBox);
        }
    }
}
