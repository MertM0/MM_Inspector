using UnityEditor;

namespace MM.Inspector.Editor
{
    public static class MMUiState
    {
        public const string GroupScope = "Group";
        public const string ButtonScope = "Button";
        public const string TabScope = "Tab";

        private const string Prefix = "MM_Inspector";

        public static string Key(string scope, MMObjectKey owner, string path)
        {
            return $"{Prefix}.{scope}.{owner}.{path}";
        }

        public static bool GetExpanded(string key, bool fallback)
        {
            return SessionState.GetBool(key, fallback);
        }

        public static void SetExpanded(string key, bool value)
        {
            SessionState.SetBool(key, value);
        }

        public static int GetTab(string key)
        {
            return SessionState.GetInt(key, 0);
        }

        public static void SetTab(string key, int value)
        {
            SessionState.SetInt(key, value);
        }
    }
}
