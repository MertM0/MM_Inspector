using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    public static class MMMessage
    {
        private static readonly Dictionary<string, GUIContent> Contents = new Dictionary<string, GUIContent>();

        public static void Draw(Rect position, GUIContent label, string message)
        {
            EditorGUI.LabelField(position, label, Get(message));
        }

        public static GUIContent Get(string message)
        {
            if (Contents.TryGetValue(message, out GUIContent content))
            {
                return content;
            }

            content = new GUIContent(message);
            Contents[message] = content;

            return content;
        }
    }
}
