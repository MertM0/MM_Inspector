using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MM.Inspector.Workflow.Editor
{
    public static class MMFlattenedIds
    {
        private const int SceneObject = 2;
        private const ulong LocalIdMask = 0x7FFFFFFFFFFFFFFF;

        private static readonly Dictionary<GUID, Dictionary<ulong, Object>> _scenes =
            new Dictionary<GUID, Dictionary<ulong, Object>>();

        private static readonly List<Component> _components = new List<Component>();

        public static bool IsFlattened(GlobalObjectId id)
        {
            return id.identifierType == SceneObject && id.targetPrefabId == 0;
        }

        public static ulong Flatten(GlobalObjectId id)
        {
            return id.targetPrefabId == 0 ? id.targetObjectId : (id.targetObjectId ^ id.targetPrefabId) & LocalIdMask;
        }

        public static Object Find(GlobalObjectId id)
        {
            if (!IsFlattened(id))
            {
                return null;
            }

            Dictionary<ulong, Object> index = Index(id.assetGUID);

            return index != null && index.TryGetValue(id.targetObjectId, out Object found) ? found : null;
        }

        public static void Release()
        {
            _scenes.Clear();
        }

        private static Dictionary<ulong, Object> Index(GUID sceneGuid)
        {
            if (_scenes.TryGetValue(sceneGuid, out Dictionary<ulong, Object> index))
            {
                return index;
            }

            Scene scene = LoadedScene(sceneGuid);

            if (!scene.IsValid())
            {
                return null;
            }

            index = Build(scene);
            _scenes[sceneGuid] = index;
            return index;
        }

        private static Scene LoadedScene(GUID sceneGuid)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                if (scene.isLoaded && AssetDatabase.GUIDFromAssetPath(scene.path) == sceneGuid)
                {
                    return scene;
                }
            }

            return default;
        }

        private static Dictionary<ulong, Object> Build(Scene scene)
        {
            List<Object> objects = new List<Object>();
            GameObject[] roots = scene.GetRootGameObjects();

            for (int i = 0; i < roots.Length; i++)
            {
                roots[i].GetComponentsInChildren(true, _components);

                for (int j = 0; j < _components.Count; j++)
                {
                    Component component = _components[j];

                    if (component == null)
                    {
                        continue;
                    }

                    objects.Add(component);

                    if (component is Transform)
                    {
                        objects.Add(component.gameObject);
                    }
                }
            }

            _components.Clear();

            Object[] targets = objects.ToArray();
            GlobalObjectId[] ids = new GlobalObjectId[targets.Length];
            GlobalObjectId.GetGlobalObjectIdsSlow(targets, ids);

            Dictionary<ulong, Object> index = new Dictionary<ulong, Object>();

            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i].targetPrefabId != 0)
                {
                    index[Flatten(ids[i])] = targets[i];
                }
            }

            return index;
        }
    }
}
