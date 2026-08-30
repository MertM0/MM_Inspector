using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace MM.Inspector.Editor
{
    public static class MMSceneCatalog
    {
        private static readonly List<MMPickerOption> Options = new List<MMPickerOption>();
        private static readonly Dictionary<string, int> NameCounts = new Dictionary<string, int>();

        private static int _version = -1;

        public static IReadOnlyList<MMPickerOption> Scenes
        {
            get
            {
                Rebuild();
                return Options;
            }
        }

        private static void Rebuild()
        {
            if (_version == MMEditorDataVersion.Current)
            {
                return;
            }

            _version = MMEditorDataVersion.Current;

            Options.Clear();
            NameCounts.Clear();

            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

            foreach (EditorBuildSettingsScene scene in scenes)
            {
                if (!scene.enabled)
                {
                    continue;
                }

                string name = Path.GetFileNameWithoutExtension(scene.path);
                NameCounts.TryGetValue(name, out int seen);
                NameCounts[name] = seen + 1;
            }

            int buildIndex = 0;

            foreach (EditorBuildSettingsScene scene in scenes)
            {
                if (!scene.enabled)
                {
                    continue;
                }

                string name = Path.GetFileNameWithoutExtension(scene.path);
                string label = NameCounts[name] > 1 ? Qualify(scene.path, name) : name;

                Options.Add(new MMPickerOption(label, name, buildIndex));
                buildIndex++;
            }
        }

        private static string Qualify(string path, string name)
        {
            string folder = Path.GetFileName(Path.GetDirectoryName(path));

            return string.IsNullOrEmpty(folder) ? name : folder + "/" + name;
        }
    }
}
