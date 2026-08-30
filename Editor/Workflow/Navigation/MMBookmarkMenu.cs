using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMBookmarkMenu
    {
        private const string HierarchyPath = "GameObject/Add Bookmark";
        private const string AssetPath = "Assets/Add Bookmark";

        [MenuItem(HierarchyPath, false, 20)]
        private static void AddFromHierarchy()
        {
            AddSelection();
        }

        [MenuItem(HierarchyPath, true)]
        private static bool ValidateHierarchy()
        {
            return Selection.activeObject != null;
        }

        [MenuItem(AssetPath, false, 20)]
        private static void AddFromProject()
        {
            AddSelection();
        }

        [MenuItem(AssetPath, true)]
        private static bool ValidateProject()
        {
            return Selection.activeObject != null;
        }

        private static void AddSelection()
        {
            Object[] targets = Selection.objects;

            for (int i = 0; i < targets.Length; i++)
            {
                MMBookmarkStore.Add(targets[i]);
            }
        }
    }
}
