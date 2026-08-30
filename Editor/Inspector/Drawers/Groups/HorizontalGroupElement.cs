using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Editor
{
    internal sealed class HorizontalGroupElement : MMFramedGroupElement
    {
        private const float ColumnSpacing = 4f;
        private const float LabelRatio = 0.42f;

        private readonly List<MMElement> _columns = new List<MMElement>();

        public HorizontalGroupElement(MMGroupContext context) : base(context)
        {
        }

        protected override float GetContentHeight(float width)
        {
            List<MMElement> columns = GetVisibleColumns();
            if (columns.Count == 0)
            {
                return 0f;
            }

            float columnWidth = GetColumnWidth(width, columns.Count);
            float tallest = 0f;

            for (int i = 0; i < columns.Count; i++)
            {
                tallest = Mathf.Max(tallest, columns[i].GetHeight(columnWidth));
            }

            return tallest;
        }

        protected override void DrawContent(Rect rect)
        {
            List<MMElement> columns = GetVisibleColumns();
            if (columns.Count == 0)
            {
                return;
            }

            float columnWidth = GetColumnWidth(rect.width, columns.Count);
            float previousLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = columnWidth * LabelRatio;

            float x = rect.x;

            for (int i = 0; i < columns.Count; i++)
            {
                float height = columns[i].GetHeight(columnWidth);
                columns[i].OnGUI(new Rect(x, rect.y, columnWidth, height));
                x += columnWidth + ColumnSpacing;
            }

            EditorGUIUtility.labelWidth = previousLabelWidth;
        }

        private List<MMElement> GetVisibleColumns()
        {
            _columns.Clear();

            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i].IsVisible)
                {
                    _columns.Add(Children[i]);
                }
            }

            return _columns;
        }

        private static float GetColumnWidth(float width, int count)
        {
            return (width - ColumnSpacing * (count - 1)) / count;
        }
    }
}
