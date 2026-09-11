using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MM.Inspector.Workflow.Editor
{
    [InitializeOnLoad]
    public static class MMClipboardHost
    {
        private const string ElementName = "mm-clipboard-paste";
        private const string AnchorClass = "unity-inspector-add-component-button";
        private const float ButtonWidth = 230f;
        private const float ButtonHeight = 22f;
        private const float RowHeight = 30f;

        private static readonly MMInspectorHost Host = new MMInspectorHost(Attach);

        static MMClipboardHost()
        {
            MMMarks.Clipboard.Changed += Sync;
        }

        private static void Sync()
        {
            Host.Sync();
        }

        private static bool Attach(EditorWindow window)
        {
            VisualElement root = window.rootVisualElement;
            VisualElement anchor = root?.Q<VisualElement>(null, AnchorClass);

            if (anchor?.parent == null)
            {
                return true;
            }

            VisualElement row = root.Q<VisualElement>(ElementName);

            if (row == null || row.parent != anchor.parent)
            {
                row = new IMGUIContainer(Draw) { name = ElementName };
                anchor.parent.Insert(anchor.parent.IndexOf(anchor) + 1, row);
            }

            bool visible = MMWorkflowSettings.Clipboard.Value && MMMarks.Clipboard.Count > 0;

            row.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            row.style.height = visible ? RowHeight : 0f;

            return true;
        }

        private static void Draw()
        {
            int count = MMMarks.Clipboard.Count;

            if (count == 0 || Selection.gameObjects.Length == 0)
            {
                return;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button(Label(count), GUILayout.Width(ButtonWidth), GUILayout.Height(ButtonHeight)))
                {
                    MMClipboardPaste.Into(Selection.gameObjects);
                }

                GUILayout.FlexibleSpace();
            }
        }

        private static string Label(int count)
        {
            return count == 1 ? "Paste 1 component" : "Paste " + count + " components";
        }
    }
}
