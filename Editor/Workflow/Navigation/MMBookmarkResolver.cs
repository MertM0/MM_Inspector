using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MM.Inspector.Workflow.Editor
{
    [InitializeOnLoad]
    public static class MMBookmarkResolver
    {
        private const int SceneIdentifier = 2;

        private static readonly Dictionary<string, Object> _cache = new Dictionary<string, Object>();
        private static readonly Dictionary<string, Texture> _icons = new Dictionary<string, Texture>();
        private static readonly HashSet<string> _failed = new HashSet<string>();
        private static readonly HashSet<string> _scenes = new HashSet<string>();

        private static bool _scenesDirty = true;

        static MMBookmarkResolver()
        {
            EditorApplication.hierarchyChanged += Invalidate;
            EditorApplication.projectChanged += Invalidate;
        }

        public static event System.Action Invalidated;

        public static MMBookmarkState State(string id)
        {
            GlobalObjectId parsed;

            if (string.IsNullOrEmpty(id) || !GlobalObjectId.TryParse(id, out parsed))
            {
                return MMBookmarkState.Broken;
            }

            string guid = parsed.assetGUID.ToString();

            if (parsed.identifierType == SceneIdentifier)
            {
                if (!SceneIsLoaded(guid))
                {
                    return MMBookmarkState.Unavailable;
                }

                return Resolve(id) == null ? MMBookmarkState.Broken : MMBookmarkState.Available;
            }

            if (string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(guid)))
            {
                return MMBookmarkState.Broken;
            }

            return Resolve(id) == null ? MMBookmarkState.Unavailable : MMBookmarkState.Available;
        }

        public static Object Resolve(string id)
        {
            if (string.IsNullOrEmpty(id) || _failed.Contains(id))
            {
                return null;
            }

            Object cached;

            if (_cache.TryGetValue(id, out cached) && cached != null)
            {
                return cached;
            }

            GlobalObjectId parsed;
            Object resolved = null;

            if (GlobalObjectId.TryParse(id, out parsed))
            {
                resolved = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(parsed);
            }

            if (resolved == null)
            {
                _cache.Remove(id);
                _failed.Add(id);
                return null;
            }

            _cache[id] = resolved;
            return resolved;
        }

        public static Texture IconOf(string id)
        {
            Texture icon;

            if (_icons.TryGetValue(id, out icon) && icon != null)
            {
                return icon;
            }

            icon = BuildIcon(Resolve(id));

            if (icon != null)
            {
                _icons[id] = icon;
            }

            return icon;
        }

        public static string IdOf(Object target)
        {
            if (target == null)
            {
                return null;
            }

            return GlobalObjectId.GetGlobalObjectIdSlow(target).ToString();
        }

        public static void Invalidate()
        {
            _failed.Clear();
            _icons.Clear();
            _scenesDirty = true;

            System.Action invalidated = Invalidated;

            if (invalidated != null)
            {
                invalidated();
            }
        }

        private static bool SceneIsLoaded(string guid)
        {
            if (_scenesDirty)
            {
                _scenesDirty = false;
                _scenes.Clear();

                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene scene = SceneManager.GetSceneAt(i);

                    if (scene.isLoaded && !string.IsNullOrEmpty(scene.path))
                    {
                        _scenes.Add(AssetDatabase.AssetPathToGUID(scene.path));
                    }
                }
            }

            return _scenes.Contains(guid);
        }

        private static Texture BuildIcon(Object target)
        {
            if (target == null)
            {
                return null;
            }

            GameObject go = target as GameObject;

            if (go == null)
            {
                return AssetPreview.GetMiniThumbnail(target);
            }

            Component[] components = go.GetComponents<Component>();

            for (int i = 0; i < components.Length; i++)
            {
                Component component = components[i];

                if (component == null || component is Transform)
                {
                    continue;
                }

                Texture icon = EditorGUIUtility.ObjectContent(component, component.GetType()).image;

                if (icon != null)
                {
                    return icon;
                }
            }

            return AssetPreview.GetMiniThumbnail(target);
        }
    }
}
