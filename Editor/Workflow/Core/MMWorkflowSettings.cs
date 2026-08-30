using System.Collections.Generic;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMWorkflowSettings
    {
        private const string Prefix = "MM_Inspector.Workflow.";

        public const string NavigationBarKey = Prefix + "NavigationBar";
        public const string PlayModeSaveKey = Prefix + "PlayModeSave";
        public const string HideScriptFieldKey = Prefix + "HideScriptField";

        public static readonly MMBoolSetting NavigationBar =
            new MMBoolSetting(NavigationBarKey, "Navigation Bar", true);

        public static readonly MMBoolSetting PlayModeSave =
            new MMBoolSetting(PlayModeSaveKey, "Play Mode Save", true);

        public static readonly MMBoolSetting HideScriptField =
            new MMBoolSetting(HideScriptFieldKey, "Hide Script Field", false);

        public static readonly IReadOnlyList<MMBoolSetting> Toggles = new[]
        {
            NavigationBar,
            PlayModeSave,
            HideScriptField
        };

        public static void Reload()
        {
            for (int i = 0; i < Toggles.Count; i++)
            {
                Toggles[i].Reload();
            }
        }

        public static void Reset()
        {
            for (int i = 0; i < Toggles.Count; i++)
            {
                Toggles[i].Reset();
            }
        }
    }
}
