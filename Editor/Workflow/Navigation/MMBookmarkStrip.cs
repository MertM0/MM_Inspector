using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMBookmarkStrip
    {
        private const float WheelSpeed = 12f;
        private const float EdgeZone = 24f;
        private const float EdgeSpeed = 8f;

        private static float _scroll;
        private static string _hoveredId;
        private static int _activeIndex = -1;
        private static MMObjectId _activeId;
        private static bool _dirty = true;
        private static int _menuIndex = -1;
        private static Rect _menuRect;
        private static int _clickCount;

        public static float Scroll => _scroll;

        public static void Invalidate()
        {
            _dirty = true;
        }

        public static bool RemoveHovered()
        {
            if (string.IsNullOrEmpty(_hoveredId) || MMBookmarkDrag.Pressed || !MMInspectorWindows.MouseIsOver())
            {
                return false;
            }

            MMBookmarkStore.Remove(_hoveredId);
            _hoveredId = null;
            return true;
        }

        public static void Draw(Rect rect)
        {
            IReadOnlyList<MMBookmarkItem> items = MMBookmarkView.Items;

            TrackActive(items, rect.width);
            _scroll = MMStripLayout.ClampScroll(_scroll, items.Count, rect.width);

            GUI.BeginClip(rect);
            Handle(items, rect.size);

            if (Event.current.type == EventType.Repaint)
            {
                MMBookmarkStripPainter.Paint(items, rect, _scroll, _activeIndex);
            }

            GUI.EndClip();

            ShowPendingMenu(items, rect);
        }

        private static void TrackActive(IReadOnlyList<MMBookmarkItem> items, float width)
        {
            Object active = Selection.activeObject;
            MMObjectId id = MMObjectId.Of(active);
            bool selectionChanged = id != _activeId;

            if (!_dirty && !selectionChanged)
            {
                return;
            }

            _dirty = false;
            _activeId = id;
            _activeIndex = IndexOf(items, active);

            if (selectionChanged && _activeIndex >= 0)
            {
                _scroll = MMStripLayout.ScrollTo(_scroll, _activeIndex, width);
            }
        }

        private static int IndexOf(IReadOnlyList<MMBookmarkItem> items, Object active)
        {
            if (active == null)
            {
                return -1;
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Available && MMBookmarkResolver.Resolve(items[i].Entry.Id) == active)
                {
                    return i;
                }
            }

            return -1;
        }

        private static void Handle(IReadOnlyList<MMBookmarkItem> items, Vector2 size)
        {
            Event current = Event.current;
            int id = GUIUtility.GetControlID(FocusType.Passive);
            bool inside = new Rect(Vector2.zero, size).Contains(current.mousePosition);

            if (MMBookmarkDrag.Pressed && GUIUtility.hotControl == 0)
            {
                MMBookmarkDrag.Cancel();
            }

            switch (current.GetTypeForControl(id))
            {
                case EventType.MouseDown:
                    OnMouseDown(items, id, inside);
                    break;

                case EventType.MouseDrag:
                    OnMouseDrag(items, id, size.x);
                    break;

                case EventType.MouseUp:
                    OnMouseUp(items, id);
                    break;

                case EventType.ContextClick:
                    OnContextClick(items, size.y, inside);
                    break;

                case EventType.ScrollWheel:
                    OnScrollWheel(items, size.x, inside);
                    break;

                case EventType.Repaint:
                    OnRepaint(items, inside);
                    break;
            }
        }

        private static void OnMouseDown(IReadOnlyList<MMBookmarkItem> items, int id, bool inside)
        {
            Event current = Event.current;

            if (current.button != 0 || !inside)
            {
                return;
            }

            int index = MMStripLayout.IndexAt(current.mousePosition.x, _scroll, items.Count);

            if (index < 0)
            {
                return;
            }

            GUIUtility.hotControl = id;
            _clickCount = current.clickCount;
            MMBookmarkDrag.Press(index, current.mousePosition.x + _scroll, index * MMNavigationMetrics.Step);
            current.Use();
        }

        private static void OnMouseDrag(IReadOnlyList<MMBookmarkItem> items, int id, float width)
        {
            if (GUIUtility.hotControl != id)
            {
                return;
            }

            Event current = Event.current;

            AutoScroll(current.mousePosition.x, width, items.Count);
            MMBookmarkDrag.Move(current.mousePosition.x + _scroll, MMNavigationMetrics.Step, items.Count);

            current.Use();
            MMInspectorWindows.RepaintHovered();
        }

        private static void OnMouseUp(IReadOnlyList<MMBookmarkItem> items, int id)
        {
            if (GUIUtility.hotControl != id)
            {
                return;
            }

            GUIUtility.hotControl = 0;

            bool dragged = MMBookmarkDrag.Active;
            int index = MMBookmarkDrag.Index;
            int slot = MMBookmarkDrag.Release();

            if (index >= 0 && index < items.Count)
            {
                if (slot >= 0)
                {
                    Reorder(items[index].StoreIndex, slot);
                }
                else if (!dragged)
                {
                    Activate(items[index]);
                }
            }

            Event.current.Use();
            MMInspectorWindows.RepaintHovered();
        }

        private static void Reorder(int from, int slot)
        {
            MMBookmarkStore.MoveTo(from, MMBookmarkView.StoreSlot(slot));
        }

        private static void OnContextClick(IReadOnlyList<MMBookmarkItem> items, float height, bool inside)
        {
            if (!inside)
            {
                return;
            }

            int index = MMStripLayout.IndexAt(Event.current.mousePosition.x, _scroll, items.Count);

            if (index < 0)
            {
                return;
            }

            _menuIndex = index;
            _menuRect = MMStripLayout.IconRect(index, _scroll, height);
            Event.current.Use();
        }

        private static void OnScrollWheel(IReadOnlyList<MMBookmarkItem> items, float width, bool inside)
        {
            if (!inside)
            {
                return;
            }

            _scroll = MMStripLayout.ClampScroll(_scroll + Event.current.delta.y * WheelSpeed, items.Count, width);
            Event.current.Use();
        }

        private static void OnRepaint(IReadOnlyList<MMBookmarkItem> items, bool inside)
        {
            int index = inside
                ? MMStripLayout.IndexAt(Event.current.mousePosition.x, _scroll, items.Count)
                : -1;

            _hoveredId = index < 0 ? null : items[index].Entry.Id;
        }

        private static void AutoScroll(float localX, float width, int count)
        {
            if (!MMBookmarkDrag.Active)
            {
                return;
            }

            if (localX < EdgeZone)
            {
                _scroll -= EdgeSpeed;
            }
            else if (localX > width - EdgeZone)
            {
                _scroll += EdgeSpeed;
            }
            else
            {
                return;
            }

            _scroll = MMStripLayout.ClampScroll(_scroll, count, width);
        }

        private static void ShowPendingMenu(IReadOnlyList<MMBookmarkItem> items, Rect rect)
        {
            if (_menuIndex < 0)
            {
                return;
            }

            int index = _menuIndex;
            _menuIndex = -1;

            if (index >= items.Count)
            {
                return;
            }

            Rect anchor = new Rect(rect.x + _menuRect.x, rect.y + _menuRect.y, _menuRect.width, _menuRect.height);
            MMBookmarkContextMenu.Show(items[index].Entry, anchor);
        }

        private static void Activate(MMBookmarkItem item)
        {
            if (!item.Available)
            {
                return;
            }

            Object target = MMBookmarkResolver.Resolve(item.Entry.Id);

            if (target == null)
            {
                return;
            }

            Selection.activeObject = target;

            if (_clickCount > 1)
            {
                EditorGUIUtility.PingObject(target);
            }
        }
    }
}
