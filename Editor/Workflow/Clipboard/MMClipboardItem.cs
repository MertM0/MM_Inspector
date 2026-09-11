using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMClipboardItem : MMMarkHeaderItem
    {
        private static readonly GUIContent _copy = EditorGUIUtility.IconContent("TreeEditor.Duplicate");

        public override int Order => 5;

        public override bool IsEnabled => MMWorkflowSettings.Clipboard.Value;

        protected override MMObjectMarks Marks => MMMarks.Clipboard;

        protected override GUIContent Icon(bool marked)
        {
            return _copy;
        }

        protected override bool Applies(Object target)
        {
            return !EditorApplication.isPlaying && target is Component;
        }
    }
}
