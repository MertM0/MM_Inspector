using UnityEditor;

namespace MM.Inspector.Editor
{
    public static class MMUiState
    {
        public const string GroupScope = "Group";
        public const string ButtonScope = "Button";

        private const string Prefix = "MM_Inspector";
        private const string TabScope = "Tab";

        public static bool GetExpanded(string scope, int ownerId, string path, bool fallback)
        {
            return SessionState.GetBool(Key(scope, ownerId, path), fallback);
        }

        public static void SetExpanded(string scope, int ownerId, string path, bool value)
        {
            SessionState.SetBool(Key(scope, ownerId, path), value);
        }

        public static int GetTab(int ownerId, string path)
        {
            return SessionState.GetInt(Key(TabScope, ownerId, path), 0);
        }

        public static void SetTab(int ownerId, string path, int value)
        {
            SessionState.SetInt(Key(TabScope, ownerId, path), value);
        }

        private static string Key(string scope, int ownerId, string path)
        {
            return $"{Prefix}.{scope}.{ownerId}.{path}";
        }
    }
}
