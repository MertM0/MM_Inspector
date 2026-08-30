using UnityEditor;

namespace MM.Inspector.Editor
{
    [InitializeOnLoad]
    public static class MMEditorDataVersion
    {
        public static int Current { get; private set; }

        static MMEditorDataVersion()
        {
            EditorApplication.projectChanged += Bump;
            EditorBuildSettings.sceneListChanged += Bump;
        }

        private static void Bump()
        {
            Current++;
        }
    }
}
