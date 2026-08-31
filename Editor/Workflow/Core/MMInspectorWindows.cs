using System;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMInspectorWindows
    {
        private const string WindowType = "InspectorWindow";
        private const string EditorType = "PropertyEditor";

        private static readonly Action<EditorWindow> _repaint = Repaint;

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
            ForEach(_repaint);
        }

        public static void ForEach(Action<EditorWindow> action)
        {
            EditorWindow[] windows = Resources.FindObjectsOfTypeAll<EditorWindow>();

            for (int i = 0; i < windows.Length; i++)
            {
                if (Is(windows[i]))
                {
                    action(windows[i]);
                }
            }
        }

        private static void Repaint(EditorWindow window)
        {
            window.Repaint();
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
