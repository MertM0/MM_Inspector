using System;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMInspectorWindows
    {
        private const string WindowType = "InspectorWindow";
        private const string EditorType = "PropertyEditor";

        public static bool MouseIsOver()
        {
            return Is(EditorWindow.mouseOverWindow);
        }

        public static void RepaintHovered()
        {
            EditorWindow window = EditorWindow.mouseOverWindow;

            if (Is(window))
            {
                window.Repaint();
            }
        }

        public static void Repaint()
        {
            EditorWindow[] windows = Resources.FindObjectsOfTypeAll<EditorWindow>();

            for (int i = 0; i < windows.Length; i++)
            {
                if (Is(windows[i]))
                {
                    windows[i].Repaint();
                }
            }
        }

        private static bool Is(EditorWindow window)
        {
            if (window == null)
            {
                return false;
            }

            for (Type type = window.GetType(); type != null; type = type.BaseType)
            {
                if (type.Name == WindowType || type.Name == EditorType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
