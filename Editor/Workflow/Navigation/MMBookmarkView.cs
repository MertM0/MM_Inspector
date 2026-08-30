using System.Collections.Generic;
using UnityEditor;

namespace MM.Inspector.Workflow.Editor
{
    [InitializeOnLoad]
    public static class MMBookmarkView
    {
        private static readonly List<MMBookmarkItem> _items = new List<MMBookmarkItem>();

        private static bool _dirty = true;
        private static bool _hidden;

        static MMBookmarkView()
        {
            MMBookmarkResolver.Invalidated += OnInvalidated;
            MMBookmarkStore.Changed += Invalidate;
        }

        public static IReadOnlyList<MMBookmarkItem> Items
        {
            get
            {
                Rebuild();
                return _items;
            }
        }

        public static void Invalidate()
        {
            _dirty = true;
        }

        public static int StoreSlot(int slot)
        {
            Rebuild();
            return slot >= 0 && slot < _items.Count ? _items[slot].StoreIndex : MMBookmarkStore.Entries.Count;
        }

        private static void OnInvalidated()
        {
            _dirty = true;
            Prune();
        }

        private static void Prune()
        {
            if (EditorApplication.isUpdating || EditorApplication.isCompiling ||
                EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            IReadOnlyList<MMBookmarkEntry> entries = MMBookmarkStore.Entries;

            for (int i = entries.Count - 1; i >= 0; i--)
            {
                if (MMBookmarkResolver.State(entries[i].Id) == MMBookmarkState.Broken)
                {
                    MMBookmarkStore.Remove(entries[i].Id);
                }
            }
        }

        private static void Rebuild()
        {
            bool hidden = MMNavigationMetrics.HideUnavailable.Value;

            if (!_dirty && _hidden == hidden)
            {
                return;
            }

            _dirty = false;
            _hidden = hidden;
            _items.Clear();

            IReadOnlyList<MMBookmarkEntry> entries = MMBookmarkStore.Entries;

            for (int i = 0; i < entries.Count; i++)
            {
                MMBookmarkState state = MMBookmarkResolver.State(entries[i].Id);

                if (state == MMBookmarkState.Broken)
                {
                    continue;
                }

                bool available = state == MMBookmarkState.Available;

                if (!available && hidden)
                {
                    continue;
                }

                _items.Add(new MMBookmarkItem(entries[i], i, available));
            }
        }
    }
}
