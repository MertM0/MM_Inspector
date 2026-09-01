using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMHoverTracker : MMHeaderItem
    {
        private const float RowPadding = 3f;

        private static MMObjectId _hovered;

        public override int Order => -100;

        public override bool IsEnabled => true;

        public static Object Hovered
        {
            get
            {
                if (_hovered == MMObjectId.None || !MMInspectorWindows.MouseIsOver())
                {
                    return null;
                }

                return _hovered.Resolve();
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
            MMObjectId id = MMObjectId.Of(targets[0]);

            if (row.Contains(Event.current.mousePosition))
            {
                _hovered = id;
            }
            else if (_hovered == id)
            {
                _hovered = MMObjectId.None;
            }

            return false;
        }
    }
}
