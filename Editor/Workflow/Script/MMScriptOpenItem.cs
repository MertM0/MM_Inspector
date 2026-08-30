using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public sealed class MMScriptOpenItem : MMHeaderItem
    {
        private const float FoldoutWidth = 13f;
        private const float Spacing = 4f;
        private const float IconWidth = 16f;

        public override int Order => 10;

        public override bool IsEnabled => MMWorkflowSettings.HideScriptField.Value;

        public override bool OnGUI(Rect rect, Object[] targets)
        {
            if (!IsEnabled)
            {
                return false;
            }

            if (targets == null || targets.Length == 0 || targets[0] == null)
            {
                return false;
            }

            MonoScript script = ScriptOf(targets[0]);

            if (script == null)
            {
                return false;
            }

            Rect icon = new Rect(FoldoutWidth + Spacing, rect.y, IconWidth, IconWidth);

            if (GUI.Button(icon, GUIContent.none, GUIStyle.none))
            {
                AssetDatabase.OpenAsset(script);
            }

            EditorGUIUtility.AddCursorRect(icon, MouseCursor.Link);
            return false;
        }

        private static MonoScript ScriptOf(Object target)
        {
            MonoBehaviour behaviour = target as MonoBehaviour;

            if (behaviour != null)
            {
                return MonoScript.FromMonoBehaviour(behaviour);
            }

            ScriptableObject scriptable = target as ScriptableObject;

            if (scriptable != null)
            {
                return MonoScript.FromScriptableObject(scriptable);
            }

            return null;
        }
    }
}
