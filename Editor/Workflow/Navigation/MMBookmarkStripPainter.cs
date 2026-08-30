using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMBookmarkStripPainter
    {
        private const float DropLineWidth = 2f;
        private const float GhostAlpha = 0.35f;
        private const float UnavailableAlpha = 0.4f;
        private const float HintAlpha = 0.5f;

        private static readonly GUIContent _missing = new GUIContent("?");
        private static readonly GUIContent _box = new GUIContent();
        private static readonly GUIContent _hint = new GUIContent("Drag objects here to bookmark");
        private static readonly GUIContent _placeholder = EditorGUIUtility.IconContent("GameObject Icon");

        public static void Paint(IReadOnlyList<MMBookmarkItem> items, Rect rect, float scroll, int activeIndex)
        {
            if (items.Count == 0)
            {
                PaintHint(rect);
            }

            for (int i = 0; i < items.Count; i++)
            {
                Rect box = MMStripLayout.IconRect(i, scroll, rect.height);

                if (box.xMax < 0f || box.x > rect.width)
                {
                    continue;
                }

                bool ghost = MMBookmarkDrag.Active && i == MMBookmarkDrag.Index;
                Paint(items[i], box, i == activeIndex, false, ghost ? GhostAlpha : 1f);
            }

            PaintDrop(items, rect, scroll);
        }

        private static void PaintHint(Rect rect)
        {
            Color color = GUI.color;
            GUI.color = new Color(color.r, color.g, color.b, color.a * HintAlpha);
            MMNavigationStyles.Hint.Draw(new Rect(0f, 0f, rect.width, rect.height), _hint, false, false, false, false);
            GUI.color = color;
        }

        private static void Paint(MMBookmarkItem item, Rect box, bool selected, bool pressed, float alpha)
        {
            Color color = GUI.color;

            if (!item.Available)
            {
                alpha *= UnavailableAlpha;
            }

            if (alpha < 1f)
            {
                GUI.color = new Color(color.r, color.g, color.b, color.a * alpha);
            }

            Texture image = item.Available ? MMBookmarkResolver.IconOf(item.Entry.Id) : _placeholder.image;

            if (image == null)
            {
                _missing.tooltip = Tooltip(item);
                MMNavigationStyles.Icon.Draw(box, _missing, false, pressed, selected, false);
            }
            else
            {
                _box.tooltip = Tooltip(item);
                MMNavigationStyles.Icon.Draw(box, _box, false, pressed, selected, false);
                GUI.DrawTexture(MMStripLayout.ImageRect(box), image, ScaleMode.ScaleToFit);
            }

            GUI.color = color;
        }

        private static void PaintDrop(IReadOnlyList<MMBookmarkItem> items, Rect rect, float scroll)
        {
            if (MMBookmarkDrag.Active)
            {
                PaintDropLine(MMBookmarkDrag.Slot, rect, scroll);
                PaintGhost(items, rect, scroll);
                return;
            }

            if (MMBookmarkDrop.Active)
            {
                PaintDropLine(MMBookmarkDrop.Slot, rect, scroll);
            }
        }

        private static void PaintDropLine(int slot, Rect rect, float scroll)
        {
            float x = MMStripLayout.DropLineX(slot, scroll, rect.width, DropLineWidth);
            EditorGUI.DrawRect(new Rect(x, 0f, DropLineWidth, rect.height), MMNavigationStyles.DropLine);
        }

        private static void PaintGhost(IReadOnlyList<MMBookmarkItem> items, Rect rect, float scroll)
        {
            int index = MMBookmarkDrag.Index;

            if (index < 0 || index >= items.Count)
            {
                return;
            }

            Rect box = MMStripLayout.IconRect(index, scroll, rect.height);
            box.x = Event.current.mousePosition.x - MMBookmarkDrag.Grab;
            Paint(items[index], box, false, true, 1f);
        }

        private static string Tooltip(MMBookmarkItem item)
        {
            MMBookmarkEntry entry = item.Entry;

            if (!string.IsNullOrEmpty(entry.Label))
            {
                return item.Available ? entry.Label : entry.Label + " (unavailable)";
            }

            Object target = item.Available ? MMBookmarkResolver.Resolve(entry.Id) : null;
            return target == null ? entry.Id : target.name;
        }
    }
}
