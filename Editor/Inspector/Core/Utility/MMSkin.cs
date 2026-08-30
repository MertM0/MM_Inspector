using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMSkin
    {
        public static Color Border =>
            EditorGUIUtility.isProSkin ? new Color(0.12f, 0.12f, 0.12f) : new Color(0.45f, 0.45f, 0.45f);

        public static Color Track =>
            EditorGUIUtility.isProSkin ? new Color(0.22f, 0.22f, 0.22f) : new Color(0.68f, 0.68f, 0.68f);

        public static Color Accent =>
            EditorGUIUtility.isProSkin ? new Color(0.24f, 0.49f, 0.79f) : new Color(0.32f, 0.55f, 0.82f);

        public static Color Text =>
            EditorGUIUtility.isProSkin ? Color.white : Color.black;
    }
}
