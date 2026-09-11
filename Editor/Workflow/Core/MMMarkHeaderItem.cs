using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public abstract class MMMarkHeaderItem : MMHeaderItem
    {
        private static readonly Color MarkedTint = new Color(0.5f, 1f, 0.5f, 1f);

        protected abstract MMObjectMarks Marks { get; }

        protected abstract GUIContent Icon(bool marked);

        protected abstract bool Applies(Object target);

        public override bool OnGUI(Rect rect, Object[] targets)
        {
            if (!IsEnabled || targets == null || targets.Length == 0 || targets[0] == null)
            {
                return false;
            }

            if (!Applies(targets[0]))
            {
                return false;
            }

            bool marked = Marks.Contains(targets[0]);
            Color previous = GUI.color;

            if (marked)
            {
                GUI.color = MarkedTint;
            }

            bool pressed = GUI.Button(rect, Icon(marked), GUIStyle.none);
            GUI.color = previous;

            if (pressed)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    Marks.Toggle(targets[i]);
                }
            }

            return true;
        }
    }
}
