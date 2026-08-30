using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMBookmarkDrop
    {
        private static bool _active;
        private static int _slot;

        public static bool Active => _active;

        public static int Slot => _slot;

        public static void Handle(Rect strip, float scroll, int count)
        {
            Event current = Event.current;

            switch (current.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    Update(strip, scroll, count);
                    break;

                case EventType.DragExited:
                    _active = false;
                    break;
            }
        }

        private static void Update(Rect strip, float scroll, int count)
        {
            Event current = Event.current;
            Object[] dragged = DragAndDrop.objectReferences;

            if (dragged.Length == 0 || !strip.Contains(current.mousePosition))
            {
                _active = false;
                return;
            }

            _active = true;
            _slot = MMBookmarkDrag.SlotAt(current.mousePosition.x - strip.x + scroll, MMNavigationMetrics.Step, count);

            DragAndDrop.visualMode = DragAndDropVisualMode.Link;

            if (current.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                Apply(dragged, _slot);
                _active = false;
            }

            current.Use();
            MMInspectorWindows.RepaintHovered();
        }

        private static void Apply(Object[] dragged, int slot)
        {
            int index = MMBookmarkView.StoreSlot(slot);

            for (int i = 0; i < dragged.Length; i++)
            {
                MMBookmarkStore.Insert(dragged[i], index + i);
            }
        }
    }
}
