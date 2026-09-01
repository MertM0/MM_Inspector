using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMPlayModeSaveItem : MMHeaderItem
    {
        private static readonly GUIContent _save = EditorGUIUtility.IconContent("SaveAs");
        private static readonly GUIContent _saved = EditorGUIUtility.IconContent("SaveActive");
        private static readonly Color _savedTint = new Color(0.5f, 1f, 0.5f, 1f);

        public override int Order => 0;

        public override bool IsEnabled => MMWorkflowSettings.PlayModeSave.Value;

        public override bool OnGUI(Rect rect, Object[] targets)
        {
            if (!EditorApplication.isPlaying)
            {
                return false;
            }

            if (!IsEnabled)
            {
                return false;
            }

            if (targets == null || targets.Length == 0 || targets[0] == null)
            {
                return false;
            }

            if (targets[0] is GameObject)
            {
                return false;
            }

            bool saved = MMPlayModeStore.Contains(MMObjectId.Of(targets[0]));
            Color previous = GUI.color;

            if (saved)
            {
                GUI.color = _savedTint;
            }

            bool pressed = GUI.Button(rect, saved ? _saved : _save, GUIStyle.none);
            GUI.color = previous;

            if (pressed)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    MMPlayModeStore.Save(targets[i]);
                }
            }

            return true;
        }
    }
}
