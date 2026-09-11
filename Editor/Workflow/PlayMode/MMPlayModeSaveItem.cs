using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMPlayModeSaveItem : MMMarkHeaderItem
    {
        private static readonly GUIContent _save = EditorGUIUtility.IconContent("SaveAs");
        private static readonly GUIContent _saved = EditorGUIUtility.IconContent("SaveActive");

        public override int Order => 0;

        public override bool IsEnabled => MMWorkflowSettings.PlayModeSave.Value;

        protected override MMObjectMarks Marks => MMMarks.PlayMode;

        protected override GUIContent Icon(bool marked)
        {
            return marked ? _saved : _save;
        }

        protected override bool Applies(Object target)
        {
            return EditorApplication.isPlaying && !(target is GameObject);
        }
    }
}
