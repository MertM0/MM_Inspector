using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public abstract class MMHeaderItem
    {
        public virtual int Order => 0;

        public abstract bool IsEnabled { get; }

        public abstract bool OnGUI(Rect rect, Object[] targets);
    }
}
