using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMComponentActions
    {
        public static bool CanDelete(Object target)
        {
            return target is Component && !(target is Transform);
        }

        public static bool CanToggleEnabled(Object target)
        {
            return target is Behaviour || target is Renderer || target is Collider;
        }

        public static void ToggleAllCollapsed()
        {
            ActiveEditorTracker tracker = ActiveEditorTracker.sharedTracker;
            UnityEditor.Editor[] editors = tracker.activeEditors;
            bool anyExpanded = false;

            for (int i = 1; i < editors.Length; i++)
            {
                if (tracker.GetVisible(i) == 1)
                {
                    anyExpanded = true;
                    break;
                }
            }

            int visible = anyExpanded ? 0 : 1;

            for (int i = 1; i < editors.Length; i++)
            {
                tracker.SetVisible(i, visible);
            }

            RepaintAll();
        }

        public static void CollapseAllExcept(Object target)
        {
            if (target == null)
            {
                return;
            }

            ActiveEditorTracker tracker = ActiveEditorTracker.sharedTracker;
            UnityEditor.Editor[] editors = tracker.activeEditors;

            for (int i = 1; i < editors.Length; i++)
            {
                bool keep = editors[i] != null && editors[i].target == target;
                tracker.SetVisible(i, keep ? 1 : 0);
            }

            RepaintAll();
        }

        public static void ToggleEnabled(Object target)
        {
            if (!CanToggleEnabled(target))
            {
                return;
            }

            Undo.RecordObject(target, "Toggle Component");

            Behaviour behaviour = target as Behaviour;

            if (behaviour != null)
            {
                behaviour.enabled = !behaviour.enabled;
            }
            else
            {
                Renderer renderer = target as Renderer;

                if (renderer != null)
                {
                    renderer.enabled = !renderer.enabled;
                }
                else
                {
                    Collider collider = target as Collider;
                    collider.enabled = !collider.enabled;
                }
            }

            EditorUtility.SetDirty(target);
            RepaintAll();
        }

        public static void Delete(Object target)
        {
            if (!CanDelete(target))
            {
                return;
            }

            Undo.DestroyObjectImmediate(target);
            RepaintAll();
        }

        private static void RepaintAll()
        {
            MMInspectorWindows.Repaint();
            EditorApplication.RepaintHierarchyWindow();
            SceneView.RepaintAll();
        }
    }
}
