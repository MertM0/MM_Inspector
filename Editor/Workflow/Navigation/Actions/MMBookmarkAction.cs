using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public abstract class MMBookmarkAction
    {
        public virtual int Order => 0;

        public abstract string Label { get; }

        public virtual bool IsEnabled(MMBookmarkEntry entry)
        {
            return entry != null;
        }

        public abstract void Execute(MMBookmarkEntry entry, Rect anchor);
    }
}
