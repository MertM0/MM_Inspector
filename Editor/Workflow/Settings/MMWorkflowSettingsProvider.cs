using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMWorkflowSettingsProvider
    {
        private const float LabelWidth = 200f;
        private const float ResetWidth = 140f;
        private const string ResetTitle = "Reset Workflow Settings";
        private const string ResetMessage = "All MM Inspector workflow settings return to their defaults.";

        [SettingsProvider]
        public static SettingsProvider Create()
        {
            SettingsProvider provider = new SettingsProvider("Project/MM Inspector/Workflow", SettingsScope.Project)
            {
                label = "Workflow",
                guiHandler = OnGUI
            };

            return provider;
        }

        private static void OnGUI(string search)
        {
            EditorGUIUtility.labelWidth = LabelWidth;

            EditorGUI.BeginChangeCheck();

            MMBoolSetting navigationBar = MMWorkflowSettings.NavigationBar;
            navigationBar.Value = EditorGUILayout.Toggle(navigationBar.Label, navigationBar.Value);

            EditorGUI.indentLevel++;

            using (new EditorGUI.DisabledScope(!navigationBar.Value))
            {
                DrawNavigation();
            }

            EditorGUI.indentLevel--;

            MMBoolSetting playModeSave = MMWorkflowSettings.PlayModeSave;
            playModeSave.Value = EditorGUILayout.Toggle(playModeSave.Label, playModeSave.Value);

            MMBoolSetting hideScriptField = MMWorkflowSettings.HideScriptField;
            bool hide = EditorGUILayout.Toggle(hideScriptField.Label, hideScriptField.Value);
            bool rebuild = hide != hideScriptField.Value;
            hideScriptField.Value = hide;

            bool changed = EditorGUI.EndChangeCheck();

            EditorGUILayout.Space();

            if (DrawReset())
            {
                MMWorkflowSettings.Reset();
                MMNavigationMetrics.Reset();
                changed = true;
                rebuild = true;
            }

            if (!changed)
            {
                return;
            }

            if (rebuild)
            {
                ActiveEditorTracker.sharedTracker.ForceRebuild();
            }

            MMInspectorWindows.Repaint();
        }

        private static void DrawNavigation()
        {
            for (int i = 0; i < MMNavigationMetrics.Toggles.Count; i++)
            {
                MMBoolSetting toggle = MMNavigationMetrics.Toggles[i];
                toggle.Value = EditorGUILayout.Toggle(toggle.Label, toggle.Value);
            }

            for (int i = 0; i < MMNavigationMetrics.Sliders.Count; i++)
            {
                MMIntSetting slider = MMNavigationMetrics.Sliders[i];
                slider.Value = EditorGUILayout.IntSlider(slider.Label, slider.Value, slider.Min, slider.Max);
            }
        }

        private static bool DrawReset()
        {
            bool clicked;

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                clicked = GUILayout.Button("Reset to Defaults", GUILayout.Width(ResetWidth));
            }

            return clicked && EditorUtility.DisplayDialog(ResetTitle, ResetMessage, "Reset", "Cancel");
        }
    }
}
