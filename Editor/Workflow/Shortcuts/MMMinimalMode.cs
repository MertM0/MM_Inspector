using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    [InitializeOnLoad]
    public static class MMMinimalMode
    {
        private const string StateKey = "MM_Inspector.Workflow.MinimalMode";

        private static bool _enabled;
        private static int[] _states;
        private static int[] _scratch;
        private static MMObjectId _last;

        static MMMinimalMode()
        {
            _enabled = SessionState.GetBool(StateKey, false);

            if (_enabled)
            {
                Subscribe();
            }
        }

        public static bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled == value)
                {
                    return;
                }

                _enabled = value;
                SessionState.SetBool(StateKey, value);

                if (value)
                {
                    Subscribe();
                    Normalize();
                }
                else
                {
                    Unsubscribe();
                }

                MMInspectorWindows.Repaint();
            }
        }

        public static void Toggle()
        {
            Enabled = !Enabled;
        }

        public static int ChooseExpanded(int[] previous, int[] current, int hovered, int last)
        {
            int first = FirstExpansion(previous, current, out int count);

            if (first < 0)
            {
                return -1;
            }

            if (hovered >= 0 && hovered < current.Length && current[hovered] == 1)
            {
                return hovered;
            }

            if (count == 1)
            {
                return first;
            }

            if (last >= 0 && last < current.Length && current[last] == 1)
            {
                return last;
            }

            int kept = OnlyExpanded(previous);

            return kept >= 0 && current[kept] == 1 ? kept : first;
        }

        private static int FirstExpansion(int[] previous, int[] current, out int count)
        {
            count = 0;

            if (current == null || previous == null || previous.Length != current.Length)
            {
                return -1;
            }

            int first = -1;

            for (int i = 0; i < current.Length; i++)
            {
                if (current[i] != 1 || previous[i] != 0)
                {
                    continue;
                }

                count++;

                if (first < 0)
                {
                    first = i;
                }
            }

            return first;
        }

        private static int OnlyExpanded(int[] states)
        {
            int found = -1;

            for (int i = 0; i < states.Length; i++)
            {
                if (states[i] != 1)
                {
                    continue;
                }

                if (found >= 0)
                {
                    return -1;
                }

                found = i;
            }

            return found;
        }

        private static void Subscribe()
        {
            _states = null;
            EditorApplication.update += OnUpdate;
        }

        private static void Unsubscribe()
        {
            _states = null;
            EditorApplication.update -= OnUpdate;
        }

        private static void OnUpdate()
        {
            ActiveEditorTracker tracker = ActiveEditorTracker.sharedTracker;
            UnityEditor.Editor[] editors = tracker.activeEditors;

            if (FirstExpansion(_states, Snapshot(tracker, editors), out _) >= 0)
            {
                Choose(tracker, editors);
            }

            Swap();
        }

        private static void Choose(ActiveEditorTracker tracker, UnityEditor.Editor[] editors)
        {
            int chosen = ChooseExpanded(
                _states,
                _scratch,
                IndexOf(editors, MMHoverTracker.Hovered),
                IndexOf(editors, _last.Resolve()));

            Object target = chosen < 0 ? null : Target(editors, chosen);

            if (target == null)
            {
                return;
            }

            Keep(target);
            Snapshot(tracker, tracker.activeEditors);
        }

        private static void Normalize()
        {
            ActiveEditorTracker tracker = ActiveEditorTracker.sharedTracker;
            UnityEditor.Editor[] editors = tracker.activeEditors;

            Object hovered = MMHoverTracker.Hovered;

            if (hovered != null && IndexOf(editors, hovered) >= 0)
            {
                Keep(hovered);
                return;
            }

            for (int i = 1; i < editors.Length; i++)
            {
                if (tracker.GetVisible(i) != 1 || editors[i] == null || editors[i].target == null)
                {
                    continue;
                }

                Keep(editors[i].target);
                return;
            }
        }

        private static void Keep(Object target)
        {
            _last = MMObjectId.Of(target);
            MMComponentActions.CollapseAllExcept(target);
        }

        private static int IndexOf(UnityEditor.Editor[] editors, Object target)
        {
            if (target == null)
            {
                return -1;
            }

            for (int i = 1; i < editors.Length; i++)
            {
                if (editors[i] != null && editors[i].target == target)
                {
                    return i - 1;
                }
            }

            return -1;
        }

        private static Object Target(UnityEditor.Editor[] editors, int index)
        {
            int slot = index + 1;

            return slot >= 0 && slot < editors.Length && editors[slot] != null ? editors[slot].target : null;
        }

        private static int[] Snapshot(ActiveEditorTracker tracker, UnityEditor.Editor[] editors)
        {
            int count = Mathf.Max(0, editors.Length - 1);

            if (_scratch == null || _scratch.Length != count)
            {
                _scratch = new int[count];
            }

            for (int i = 0; i < count; i++)
            {
                _scratch[i] = tracker.GetVisible(i + 1);
            }

            return _scratch;
        }

        private static void Swap()
        {
            int[] previous = _states;

            _states = _scratch;
            _scratch = previous;
        }
    }
}
