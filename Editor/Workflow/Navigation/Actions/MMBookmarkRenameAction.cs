using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMBookmarkRenameAction : MMBookmarkAction
    {
        public override int Order => 20;

        public override string Label => "Rename";

        public override void Execute(MMBookmarkEntry entry, Rect anchor)
        {
            PopupWindow.Show(anchor, new MMBookmarkRenamePopup(entry));
        }
    }
}
