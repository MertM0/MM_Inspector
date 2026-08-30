using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMBookmarkRemoveAction : MMBookmarkAction
    {
        public override int Order => 100;

        public override string Label => "Remove";

        public override void Execute(MMBookmarkEntry entry, Rect anchor)
        {
            MMBookmarkStore.Remove(entry.Id);
        }
    }
}
