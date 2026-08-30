using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMHoverTracker : MMHeaderItem
    {
        private const float RowPadding = 3f;

        private static int _hoveredId;

        public override int Order => -100;

        public override bool IsEnabled => true;

        public static Object Hovered
        {
            get
            {
                if (_hoveredId == 0 || !MMInspectorWindows.MouseIsOver())
                {
                    return null;
                }

                return EditorUtility.EntityIdToObject(_hoveredId);
            }
        }

        public override bool OnGUI(Rect rect, Object[] targets)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return false;
            }

            if (targets == null || targets.Length == 0 || targets[0] == null)
            {
                return false;
            }

            Rect row = new Rect(0f, rect.y - RowPadding, Screen.width, rect.height + RowPadding * 2f);
            int id = targets[0].GetInstanceID();

            if (row.Contains(Event.current.mousePosition))
            {
                _hoveredId = id;
            }
            else if (_hoveredId == id)
            {
                _hoveredId = 0;
            }

            return false;
        }
    }
}
