using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    [InitializeOnLoad]
    public static class MMSelectionHistory
    {
        private const string StateKey = "MM_Inspector.Workflow.History.2";
        private const int Capacity = 64;

        private static readonly MMHistoryStack _stack;

        static MMSelectionHistory()
        {
            _stack = MMHistoryStack.Deserialize(SessionState.GetString(StateKey, string.Empty), Capacity);
            Selection.selectionChanged += OnSelectionChanged;
        }

        public static bool CanGoBack => _stack.CanGoBack;

        public static bool CanGoForward => _stack.CanGoForward;

        public static void Back()
        {
            Navigate(_stack.GoBack());
        }

        public static void Forward()
        {
            Navigate(_stack.GoForward());
        }

        private static void Navigate(ulong raw)
        {
            if (raw == 0ul)
            {
                return;
            }

            Object target = MMObjectId.FromRaw(raw).Resolve();

            if (target == null)
            {
                return;
            }

            Selection.activeObject = target;
            Persist();
        }

        private static void OnSelectionChanged()
        {
            Object target = Selection.activeObject;

            if (target == null)
            {
                return;
            }

            _stack.Push(MMObjectId.Of(target).Raw);
            Persist();
        }

        private static void Persist()
        {
            SessionState.SetString(StateKey, _stack.Serialize());
        }
    }
}
