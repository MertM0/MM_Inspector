using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MM.Inspector.Workflow.Editor
{
    [InitializeOnLoad]
    public static class MMPlayModeStore
    {
        private static readonly List<MMPlayModeSnapshot> _snapshots = new List<MMPlayModeSnapshot>();
        private static readonly HashSet<MMObjectId> _ids = new HashSet<MMObjectId>();

        static MMPlayModeStore()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        public static int Count => _snapshots.Count;

        public static bool Contains(MMObjectId owner)
        {
            return _ids.Contains(owner);
        }

        public static void Add(MMPlayModeSnapshot snapshot)
        {
            if (snapshot == null)
            {
                return;
            }

            for (int i = 0; i < _snapshots.Count; i++)
            {
                if (_snapshots[i].Owner != snapshot.Owner)
                {
                    continue;
                }

                _snapshots[i] = snapshot;
                return;
            }

            _snapshots.Add(snapshot);
            _ids.Add(snapshot.Owner);
        }

        public static void Save(Object target)
        {
            if (target == null)
            {
                return;
            }

            string globalId = GlobalObjectId.GetGlobalObjectIdSlow(target).ToString();
            Add(new MMPlayModeSnapshot(MMObjectId.Of(target), globalId, EditorJsonUtility.ToJson(target)));
        }

        public static void Clear()
        {
            _snapshots.Clear();
            _ids.Clear();
        }

        public static int Restore()
        {
            int restored = 0;

            for (int i = 0; i < _snapshots.Count; i++)
            {
                if (Apply(_snapshots[i]))
                {
                    restored++;
                }
            }

            Clear();
            return restored;
        }

        private static bool Apply(MMPlayModeSnapshot snapshot)
        {
            GlobalObjectId parsed;

            if (!GlobalObjectId.TryParse(snapshot.GlobalId, out parsed))
            {
                return false;
            }

            Object target = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(parsed);

            if (target == null)
            {
                return false;
            }

            Undo.RecordObject(target, "Restore Play Mode Values");
            EditorJsonUtility.FromJsonOverwrite(snapshot.Json, target);
            EditorUtility.SetDirty(target);
            return true;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            if (change != PlayModeStateChange.EnteredEditMode)
            {
                return;
            }

            if (_snapshots.Count == 0)
            {
                return;
            }

            int restored = Restore();

            if (restored > 0)
            {
                Debug.Log("[MM_Inspector] Restored play mode values on " + restored + " component(s).");
            }
        }
    }
}
