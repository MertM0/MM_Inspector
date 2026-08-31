using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMNavigationBar
    {
        private const float BorderWidth = 1f;

        private static readonly GUIContent _back = EditorGUIUtility.IconContent("tab_prev");
        private static readonly GUIContent _forward = EditorGUIUtility.IconContent("tab_next");

        public static void Draw()
        {
            Rect row = GUILayoutUtility.GetRect(0f, MMNavigationMetrics.RowHeight, GUILayout.ExpandWidth(true));

            if (Event.current.type == EventType.Layout)
            {
                return;
            }

            Rect content = Content(row);

            if (content.width <= 0f)
            {
                return;
            }

            float button = MMNavigationMetrics.IconSize.Value;
            float gap = MMNavigationMetrics.SectionGap.Value;
            float x = content.x;

            if (MMNavigationMetrics.HistoryArrows.Value)
            {
                DrawHistory(content, button, ref x);
                x += gap;
            }

            Rect strip = new Rect(x, content.y, Mathf.Max(0f, content.xMax - x), content.height);

            MMBookmarkStrip.Draw(strip);
            MMBookmarkDrop.Handle(strip, MMBookmarkStrip.Scroll, MMBookmarkView.Items.Count);
            DrawDropHighlight(strip);
        }

        private static Rect Content(Rect row)
        {
            float left = MMNavigationMetrics.PaddingLeft.Value;
            float right = MMNavigationMetrics.PaddingRight.Value;

            return new Rect(
                row.x + left,
                row.y + MMNavigationMetrics.PaddingTop.Value,
                row.width - left - right,
                MMNavigationMetrics.BarHeight.Value);
        }

        private static void DrawHistory(Rect content, float button, ref float x)
        {
            using (new EditorGUI.DisabledScope(!MMSelectionHistory.CanGoBack))
            {
                if (GUI.Button(new Rect(x, content.y, button, content.height), _back, MMNavigationStyles.ArrowLeft))
                {
                    MMSelectionHistory.Back();
                }
            }

            x += button;

            using (new EditorGUI.DisabledScope(!MMSelectionHistory.CanGoForward))
            {
                if (GUI.Button(new Rect(x, content.y, button, content.height), _forward, MMNavigationStyles.ArrowRight))
                {
                    MMSelectionHistory.Forward();
                }
            }

            x += button;
        }

        private static void DrawDropHighlight(Rect rect)
        {
            if (Event.current.type != EventType.Repaint || !MMBookmarkDrop.Active)
            {
                return;
            }

            Color color = MMNavigationStyles.DropLine;

            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, BorderWidth), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - BorderWidth, rect.width, BorderWidth), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, BorderWidth, rect.height), color);
            EditorGUI.DrawRect(new Rect(rect.xMax - BorderWidth, rect.y, BorderWidth, rect.height), color);
        }
    }
}
