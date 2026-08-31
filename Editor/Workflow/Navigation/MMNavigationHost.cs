using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MM.Inspector.Workflow.Editor
{
    [InitializeOnLoad]
    public static class MMNavigationHost
    {
        private const string ElementName = "mm-navigation-bar";
        private const string ContainerClass = "unity-inspector-main-container";

        private static readonly Action<EditorWindow> _refresh = Refresh;

        private static int _windows;
        private static int _ready;

        static MMNavigationHost()
        {
            EditorApplication.update += OnUpdate;
            Selection.selectionChanged += OnChanged;
            MMBookmarkResolver.Invalidated += OnChanged;
            MMBookmarkStore.Changed += OnStoreChanged;
        }

        public static bool Sync()
        {
            _windows = 0;
            _ready = 0;

            MMInspectorWindows.ForEach(_refresh);

            return _windows > 0 && _windows == _ready;
        }

        private static void OnUpdate()
        {
            if (Sync())
            {
                EditorApplication.update -= OnUpdate;
            }
        }

        private static void Refresh(EditorWindow window)
        {
            _windows++;

            if (Attach(window))
            {
                _ready++;
            }

            window.Repaint();
        }

        private static bool Attach(EditorWindow window)
        {
            VisualElement root = window.rootVisualElement;

            if (root == null || root.childCount == 0)
            {
                return false;
            }

            VisualElement bar = root.Q<VisualElement>(ElementName);

            if (bar == null)
            {
                bar = new IMGUIContainer(MMNavigationBar.Draw) { name = ElementName };
                root.Insert(Slot(root), bar);
            }

            Layout(bar);

            return true;
        }

        private static int Slot(VisualElement root)
        {
            VisualElement container = root.Q<VisualElement>(null, ContainerClass);

            return container != null && container.parent == root ? root.IndexOf(container) : 0;
        }

        private static void Layout(VisualElement bar)
        {
            bool visible = MMWorkflowSettings.NavigationBar.Value;
            float height = visible ? MMNavigationMetrics.RowHeight : 0f;

            bar.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;

            if (!Mathf.Approximately(bar.resolvedStyle.height, height))
            {
                bar.style.height = height;
            }
        }

        private static void OnChanged()
        {
            Sync();
        }

        private static void OnStoreChanged()
        {
            MMBookmarkStrip.Invalidate();
            Sync();
        }
    }
}
