using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMNavigationStyles
    {
        private static GUIStyle _icon;
        private static GUIStyle _arrowLeft;
        private static GUIStyle _arrowRight;
        private static GUIStyle _hint;
        private static bool _pro;

        public static GUIStyle Icon => Cached(ref _icon, EditorStyles.miniButton, TextAnchor.MiddleCenter);

        public static GUIStyle ArrowLeft => Cached(ref _arrowLeft, EditorStyles.miniButtonLeft, TextAnchor.MiddleCenter);

        public static GUIStyle ArrowRight => Cached(ref _arrowRight, EditorStyles.miniButtonRight, TextAnchor.MiddleCenter);

        public static GUIStyle Hint => Cached(ref _hint, EditorStyles.miniLabel, TextAnchor.MiddleLeft);

        public static Color DropLine => EditorGUIUtility.isProSkin
            ? new Color(0.35f, 0.65f, 1f)
            : new Color(0.15f, 0.4f, 0.85f);

        private static GUIStyle Cached(ref GUIStyle style, GUIStyle source, TextAnchor alignment)
        {
            if (_pro != EditorGUIUtility.isProSkin)
            {
                _pro = EditorGUIUtility.isProSkin;
                _icon = null;
                _arrowLeft = null;
                _arrowRight = null;
                _hint = null;
                style = null;
            }

            return style ?? (style = new GUIStyle(source)
            {
                fixedHeight = 0f,
                fixedWidth = 0f,
                margin = new RectOffset(),
                padding = new RectOffset(),
                alignment = alignment
            });
        }
    }
}
